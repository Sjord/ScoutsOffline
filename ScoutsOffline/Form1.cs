using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScoutsOffline.Sol;
using System.Diagnostics;
using ScoutsOffline.Model;

namespace ScoutsOffline
{
    public partial class Form1 : Form
    {
        UserModelRepository repository;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            /*/
            var username = "Sjoerder";
            repository = new UserModelRepository(username);
            FilterDataSource();
            return;
             //*/

            var login = new Login();
            login.LoginClick += OnLoginClick;
            login.Show(this);
            login.Focus();
        }

        protected void OnLoginClick(object sender, LoginClickEventArgs eArgs)
        {
            ((Login)sender).Hide();

            this.sol = new ScoutsOnLine();
            if (sol.Authenticate(eArgs.username, eArgs.password))
            {
                var username = eArgs.username;
                repository = new UserModelRepository(username);
                FilterDataSource();

                progressBar1.Style = ProgressBarStyle.Marquee;
                var getAllMembers = new GetAllMembersDelegate(GetAllMembers);
                getAllMembers.BeginInvoke(sol, null, null);

                repository.Model.RoleList = new RoleList(sol.roles);
                repository.Model.UserId = sol.userId;
            }
            else
            {
                ((Login)sender).Show();
            }
        }

        private delegate void GetAllMembersDelegate(ScoutsOnLine sol);

        private void GetAllMembers(ScoutsOnLine sol)
        {
            sol.StartGetMembers(AddMembers);
        }

        private void AddMembers(List<Member> newMembers, int step, int count)
        {
            repository.Model.MemberList.UpdateWith(newMembers);
            repository.Store();
            
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new MethodInvoker(delegate {
                    dataGridView1.DataSource = null;
                    FilterDataSource();

                    // For performance
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                }));
            }
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new MethodInvoker(delegate
                {
                    progressBar1.Style = ProgressBarStyle.Continuous;
                    progressBar1.Maximum = count;
                    progressBar1.Value = step + 1;
                    progressBar1.Visible = step < count - 1;
                }));
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var members = (List<Member>)dataGridView1.DataSource;
            var member = members[e.RowIndex];
            var lidnummer = member.Lidnummer;
            var link = string.Format("https://sol.scouting.nl/index.php?task=ma_person&action=view&button=btn_detail&per_id={0}", lidnummer);
            System.Diagnostics.Process.Start(link);
        }

        private string _previousSearchValue = string.Empty;
        private ScoutsOnLine sol;

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            FilterDataSource();
        }

        private void FilterDataSource() {
            var searchtext = SearchBox.Text;
            List<Member> newSource;
            if (searchtext == string.Empty)
            {
                newSource = this.repository.Model.MemberList.ToList();
            }
            else
            {
                List<Member> source;
                if (dataGridView1.DataSource != null && searchtext.StartsWith(_previousSearchValue))
                {
                    source = (List<Member>)dataGridView1.DataSource;
                }
                else
                {
                    source = this.repository.Model.MemberList.ToList();
                }
                var keywords = searchtext.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                newSource = source.Where(m => m.Matches(keywords)).ToList();
                _previousSearchValue = searchtext;
            }
            dataGridView1.DataSource = newSource;
            ResultCount.Text = string.Format("{0} resultaten", newSource.Count);
        }

        private void MarkeerLid_Click(object sender, EventArgs e)
        {
            List<int> rowIndices = new List<int>();
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                var row = cell.RowIndex;
                if (!rowIndices.Contains(row))
                    rowIndices.Add(row);
            }

            var members = (List<Member>)dataGridView1.DataSource;
            foreach (var index in rowIndices)
            {
                var member = members[index];
                repository.Model.MemberList.ToggleSelect(member);
            }
            dataGridView1.Refresh();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var members = (List<Member>)dataGridView1.DataSource;
            var member = members[e.RowIndex];
            if (member.Selected)
            {
                e.CellStyle.BackColor = Color.LightYellow;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }
        }

        private void toekennenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var kwali = new Kwalificatie(repository, sol);
            kwali.Show();
        }
    }
}
