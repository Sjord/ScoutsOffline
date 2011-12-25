using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PwdHashSharp;

namespace ScoutsOffline
{
    public partial class Login : Form
    {
        public event EventHandler<LoginClickEventArgs> LoginClick;

        bool pwdHash;

        public Login()
        {
            InitializeComponent();
            Omgeving.SelectedIndex = 0;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (LoginClick != null)
            {
                var eventArgs = new LoginClickEventArgs(username.Text, password.Text, Omgeving.Text);
                LoginClick(this, eventArgs);
            }
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                pwdHash = !pwdHash;
                password.BackColor = pwdHash ? Color.Yellow : Color.White;
            }
            if (e.KeyCode == Keys.Enter)
            {
                password_Leave(sender, e);
                loginButton_Click(sender, e);
            }
        }

        private void password_Leave(object sender, EventArgs e)
        {
            if (pwdHash || password.Text.StartsWith("@@"))
            {
                var hasher = new PasswordHasher("scouting.nl", password.Text);
                password.Text = hasher.GetHashedPassword();
                pwdHash = false;
                password.BackColor = Color.White;
            }
        }
    }
}
