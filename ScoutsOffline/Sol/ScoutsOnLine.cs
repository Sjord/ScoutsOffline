using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoutsOffline.Http;
using System.IO;
using HtmlAgilityPack;

namespace ScoutsOffline.Sol
{
    public class ScoutsOnLine
    {
        private Browser httpBrowser;
        public const string BaseUrl = "https://sol.scouting.nl/";

        public ScoutsOnLine()
        {
            httpBrowser = new Browser();
        }

        public bool Authenticate(string username, string password)
        {
            var authenticator = new Authenticator(httpBrowser);
            var response = authenticator.Authenticate(username, password);
            OnResponse(response);
            var cookie = httpBrowser.GetCookie(BaseUrl, "SOL_LOGGED_IN");
            return cookie.Value == "1";
        }

        public void OnResponse(Response response)
        {
            var document = new HtmlDocument();
            // document.Load(response.GetStream());
            document.Load(@"C:\test.html");
            var select = document.DocumentNode.SelectSingleNode("//select[name=role_id");
            var options = select.ChildNodes;
            foreach (var option in options)
            {
                var id = option.Attributes["value"];
                var text = option.InnerText;
            }
        }
    }
}
