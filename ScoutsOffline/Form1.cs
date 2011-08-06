using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScoutsOffline.Sol;

namespace ScoutsOffline
{
    public partial class Form1 : Form
    {
        List<Member> members;

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
            sol.Authenticate(eArgs.username, eArgs.password);
            members = sol.GetSelection();
            dataGridView1.DataSource = members;
            // var lidLink = dataGridView1.Columns["LidLink"];
            // dataGridView1.Columns.Remove(lidLink);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var members = (List<Member>)dataGridView1.DataSource;
            var member = members[e.RowIndex];
            // var link = member.LidLink;
            // System.Diagnostics.Process.Start(link);
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            var searchtext = SearchBox.Text;
            dataGridView1.DataSource = members.Where(m => m.Matches(searchtext)).ToList();
        }
    }
}
