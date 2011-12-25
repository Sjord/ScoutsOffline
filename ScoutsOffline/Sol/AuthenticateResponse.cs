using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Sol
{
    public class AuthenticateResponse : SolResponse
    {
        public AuthenticateResponse(Http.Response response) : base(response)
        {
            var cookie = response.GetCookie("SOL_LOGGED_IN");
            this.LoggedIn = cookie.Value == "1";
        }

        public bool LoggedIn { get; set; }
    }
}
