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

namespace ScoutsOffline
{
    public partial class Form1 : Form
    {
        List<Member> members = new List<Member>();

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var login = new Login();
            login.LoginClick += OnLoginClick;
            login.Show(this);
            login.Focus();
        }

        protected void OnLoginClick(object sender, LoginClickEventArgs eArgs)
        {
            ((Login)sender).Hide();

            var sol = new ScoutsOnLine();
            if (sol.Authenticate(eArgs.username, eArgs.password))
            {
                progressBar1.Style = ProgressBarStyle.Marquee;
                var getAllMembers = new GetAllMembersDelegate(GetAllMembers);
                getAllMembers.BeginInvoke(sol, null, null);
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
            var allMembers = new HashSet<Member>(this.members);
            allMembers.UnionWith(newMembers);
            this.members = allMembers.OrderBy(m => m.Lidachternaam).ThenBy(m => m.Lidvoornaam).ToList();
            
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

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            FilterDataSource();
        }

        private void FilterDataSource() {
            var searchtext = SearchBox.Text;
            if (searchtext == string.Empty)
            {
                dataGridView1.DataSource = this.members;
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
                    source = this.members;
                }
                var keywords = searchtext.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                dataGridView1.DataSource = source.Where(m => m.Matches(keywords)).ToList();
                _previousSearchValue = searchtext;
            }
        }
    }
}
