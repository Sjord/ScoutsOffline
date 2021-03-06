﻿using System;
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
using System.Threading;
using System.Globalization;

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

            this.sol = new ScoutsOnLine(eArgs.omgeving);
            var auth = sol.Authenticate(eArgs.username, eArgs.password);
            if (auth.LoggedIn)
            {
                var username = eArgs.username;
                repository = new UserModelRepository(username);
                FilterDataSource();

                progressBar1.Style = ProgressBarStyle.Marquee;

                var worker = new GetMembersWorker(sol);
                worker.ProgressChanged += OnMembersProgressChanged;
                worker.RunWorkerCompleted += OnMembersCompleted;
                worker.RunWorkerAsync();

                repository.Model.RoleList = new RoleList(auth.Roles);
                repository.Model.UserId = auth.UserId;
            }
            else
            {
                ((Login)sender).Show();
            }
        }

        private void OnMembersProgressChanged(object sender, ProgressChangedEventArgs eventArgs)
        {
            var percent = eventArgs.ProgressPercentage;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = percent;

            var exception = eventArgs.UserState as Exception;
            if (exception != null)
            {
                // TODO show exception
                return;
            }

            var members = eventArgs.UserState as List<Member>;
            if (members != null)
            {
                repository.Model.MemberList.UpdateWith(members);
                repository.Store();

                dataGridView1.DataSource = null;
                FilterDataSource();
            }
        }

        private void OnMembersCompleted(object sender, RunWorkerCompletedEventArgs eventArgs)
        {
            progressBar1.Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            var members = (List<Member>)dataGridView1.DataSource;
            var member = members[e.RowIndex];
            var lidnummer = member.Lidnummer;
            var link = string.Format("https://sol.scouting.nl/ma/person/{0}", lidnummer);
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

                var stopwatch = Stopwatch.StartNew();
                newSource = source.Where(m => m.Matches(keywords)).ToList();
                stopwatch.Stop();
                Debug.WriteLine(stopwatch.ElapsedMilliseconds);

                _previousSearchValue = searchtext;
            }
            dataGridView1.DataSource = newSource;

            ResultCount.Text = string.Format("{0} resultaten", newSource.Count);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // For performance
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.AutoGenerateColumns = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-NL");
        }
    }
}
