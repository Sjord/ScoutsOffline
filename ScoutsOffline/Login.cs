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

        public Login()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (LoginClick != null)
            {
                var eventArgs = new LoginClickEventArgs(username.Text, password.Text);
                LoginClick(this, eventArgs);
            }
        }

        private void PwdHash_Click(object sender, EventArgs e)
        {
            var hasher = new PasswordHasher("scouting.nl", password.Text);
            password.Text = hasher.GetHashedPassword();
        }

    }
}
