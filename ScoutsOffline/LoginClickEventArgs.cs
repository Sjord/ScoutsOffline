using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline
{
    public class LoginClickEventArgs : EventArgs
    {
        public LoginClickEventArgs(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string username { get; set; }

        public string password { get; set; }
    }
}
