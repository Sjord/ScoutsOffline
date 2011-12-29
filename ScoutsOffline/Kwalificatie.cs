using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScoutsOffline.Model;
using ScoutsOffline.Sol;

namespace ScoutsOffline
{
    public partial class Kwalificatie : Form
    {
        private UserModelRepository repository;
        private ScoutsOnLine sol;

        public Kwalificatie(UserModelRepository repository, ScoutsOnLine sol) : this()
        {
            this.repository = repository;
            this.sol = sol;
        }

        public Kwalificatie()
        {
            InitializeComponent();
        }

        private void Kwalificatie_Load(object sender, EventArgs e)
        {
            RollenCombo.DataSource = repository.Model.RoleList.ToList();
            KwalificatieCombo.DataSource = KwalificatieList.Get().ToList();
            LedenListBox.DataSource = repository.Model.MemberList.SelectedMembers;
            SetExaminator();
        }

        private void RollenCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetExaminator();
        }

        private void SetExaminator()
        {
            var role = (Role) RollenCombo.SelectedItem;
            ExaminatorCombo.DataSource = repository.Model.MemberList.Where(m => m.Organisatienummer == role.OrganisatieNr.ToString()).ToList();
            ExaminatorCombo.Items.Insert(0, string.Empty);
        }

        private void Toekennen_Click(object sender, EventArgs e)
        {
            RollenCombo.Enabled = false;
            KwalificatieCombo.Enabled = false;
            dateTimePicker1.Enabled = false;
            ExaminatorCombo.Enabled = false;
            Toekennen.Enabled = false;

            progressBar1.Maximum = repository.Model.MemberList.SelectedMembers.Count;
            progressBar1.Value = 0;

            sol.SwitchRole((Role)RollenCombo.SelectedItem);
            foreach (var member in repository.Model.MemberList.SelectedMembers)
            {
                sol.AddQualification(
                    member, 
                    (Model.Kwalificatie) KwalificatieCombo.SelectedItem, 
                    dateTimePicker1.Value, 
                    (Member) ExaminatorCombo.SelectedItem);
                progressBar1.Value++;
            }

            this.Close();
        }
    }
}
