using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoutsOffline.Http;
using System.IO;
using HtmlAgilityPack;
using System.Web;

namespace ScoutsOffline.Sol
{
    public class ScoutsOnLine
    {
        private Browser httpBrowser;
        public const string BaseUrl = "https://sol.scouting.nl/";

        List<Role> roles = null;

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
            if (this.roles == null)
            {
                this.roles = GetRoles(response);
            }
        }

        public List<Role> GetRoles(Response response) 
        {
            var document = new HtmlDocument();
            document.LoadHtml(response.Content);
            var select = document.DocumentNode.SelectSingleNode("//select[@name='role_id']");
            if (select == null)
            {
                return null;
            }
            var options = select.ChildNodes;
            List<Role> roles = new List<Role>();
            Role role = null;
            foreach (var option in options)
            {
                if (option.Name == "option")
                {
                    role = new Role();
                    role.Id = option.GetAttributeValue("value", null);
                    roles.Add(role);
                }
                if (option.NodeType == HtmlNodeType.Text)
                {
                    if (role != null)
                    {
                        role.Name = option.InnerText;
                    }
                }
            }
            return roles;
            
        }

        //public List<Member> GetMembers()
        //{
        //    Dictionary<int, Member> membersDict = new Dictionary<int,Member>();
        //    foreach (var role in roles)
        //    {
        //        SwitchRole(role);
        //        var request = new Request(ResolveUrl("index.php?task=ma_person&action=list"));
        //        var response = httpBrowser.DoRequest(request);
        //        response.Save(@"C:\members" + role.Id + ".html");
        //        //var response = Response.FromFile(@"C:\members.html");
        //        OnResponse(response);
        //        var members = ParseFilterTable(response);
        //        foreach (var member in members)
        //        {
        //            membersDict[member.Id] = member;
        //        }
        //    }
        //    return membersDict.Values.OrderBy(m => m.Lid).ToList();
        //}

        public List<Member> GetSelection()
        {
            var postData = new Dictionary<string, object>()
            {
                {"task", "sel_selection"},
                {"action", "perform"},
                {"button", "post"},
                {"sel_id", "1216"},
                //{"export_type", "1"},
                {"usr_cse_id", "3"},
                //{"submit", "Uitvoeren"},
            };
            var request = new PostRequest(ResolveUrl("index.php"), postData);
            //var request = new Request(ResolveUrl("index.php?task=sel_selection&action=perform&button=post&sel_id=1216"));
            var response = httpBrowser.DoRequest(request);
            var contents = response.Content;
            var csvReader = new CsvConverter(contents);

            List<Member> members = new List<Member>();
            var keys = csvReader.GetKeys();
            while (true) {
                List<string> values = csvReader.GetValues();
                if (values == null) break;
                Member member = new Member();
                Type memberType = typeof(Member);
                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i].Replace(" ", "");
                    var property = memberType.GetProperty(key);
                    if (property != null)
                    {
                        property.SetValue(member, values[i], null);
                    }
                }
                members.Add(member);
            }
            return members;
        }

        private List<Member> GetMemberPage()
        {
            var request = new Request(ResolveUrl("index.php?task=ma_person&action=list"));
            var response = httpBrowser.DoRequest(request);
            OnResponse(response);
            return ParseFilterTable(response);
        }

        private void SwitchRole(Role role)
        {
            var postData = new Dictionary<string, object>
            {
                {"task", "ma_function"},
                {"action", "edit"},
                {"button", "changeRole"},
                {"submit", "Wissel rol"},
                {"role_id", role.Id},
            };
            var request = new PostRequest(ResolveUrl("/index.php"), postData);
            httpBrowser.DoRequest(request);
        }

        private List<Member> ParseFilterTable(Response response)
        {
            var document = new HtmlDocument();
            document.LoadHtml(response.Content);
            var table = document.DocumentNode.SelectSingleNode("//table[@class='filter_tab_table']");
            var headers = table.SelectNodes("//input[@class='filterlink']").Select(elem => elem.GetAttributeValue("value", null)).ToList();
            var dataRows = table.SelectNodes("//tr[starts-with(@class, 'table_tr_color')]");
            List<Member> members = new List<Member>();
            var memberType = typeof(Member);
            foreach (var tr in dataRows)
            {
                var dataFields = tr.SelectNodes("./td[starts-with(@class, 'listtxt_tab')]");
                var index = 0;
                var member = new Member();
                foreach (var td in dataFields)
                {
                    var key = headers[index++];
                    var link = td.SelectSingleNode("./a");
                    string value = null;
                    if (link == null)
                    {
                        value = td.InnerText;
                    }
                    else
                    {
                        var href = link.GetAttributeValue("href", null);
                        var linkProp = memberType.GetProperty(key + "Link");
                        if (linkProp != null)
                        {
                            var url = ResolveUrl(href);
                            linkProp.SetValue(member, url, null);
                        }
                        value = link.InnerText;
                    }
                    var prop = memberType.GetProperty(key);
                    if (prop != null)
                    {
                        prop.SetValue(member, value, null);
                    }
                }
                members.Add(member);
            }
            return members;
        }

        private string ResolveUrl(string href)
        {
            href = HttpUtility.HtmlDecode(href);
            if (!href.StartsWith("http"))
            {
                var uri = new Uri(new Uri(BaseUrl), href);
                return uri.ToString();
            }
            return href;
        }
    }
}
