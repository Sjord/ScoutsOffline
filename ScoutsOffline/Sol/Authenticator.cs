using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoutsOffline.Http;

namespace ScoutsOffline.Sol
{
    class Authenticator
    {
        private Browser browser;

        public Authenticator(Browser browser)
        {
            this.browser = browser;
        }

        public Response Authenticate(string username, string password)
        {
            var url = "https://sol.scouting.nl/index.php?task=rs_user&action=login&button=";
            var postData = new Dictionary<string, object>
            {
                {"task", "rs_user"},
                {"action", "checkLogin"},
                {"button", ""},
                {"v", "check"},
                {"referer", ""},
                {"luser_name", username},
                {"luser_password", password},
                {"submit", "Log in"},
            };
            var loginRequest = new PostRequest(url, postData);
            var response = browser.DoRequest(loginRequest);
            return response;
        }
    }
}
