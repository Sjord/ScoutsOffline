using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline
{
    public class LoginClickEventArgs : EventArgs
    {
        public LoginClickEventArgs(string username, string password, string omgeving)
        {
            this.username = username;
            this.password = password;
            this.omgeving = omgeving;
        }

        public string username { get; set; }
        public string password { get; set; }
        public string omgeving { get; set; }
    }
}
