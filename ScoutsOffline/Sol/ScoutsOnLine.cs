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
        private object lockObject = new object();

        private Browser httpBrowser;
        public string BaseUrl { get; private set ; }
        public delegate void MembersAvailable(List<Member> members, int step, int count);

        internal List<Role> roles = null;

        public ScoutsOnLine(string omgeving)
        {
            httpBrowser = new Browser();
            this.BaseUrl = string.Format("https://{0}/", omgeving);
        }

        public AuthenticateResponse Authenticate(string username, string password)
        {
            var authenticator = new Authenticator(httpBrowser, this.BaseUrl);
            var response = authenticator.Authenticate(username, password);
            var authResponse = new AuthenticateResponse(response);
            this.roles = authResponse.Roles;
            return authResponse;
        }

        public void StartGetMembers(MembersAvailable callback)
        {
            int i = 0;
            foreach (var role in this.roles)
            {
                SwitchRole(role);
                var members = GetSelection();
                callback(members, i++, roles.Count);
            }
        }

        public List<Member> GetSelection()
        {
            var postData = new FormValueCollection
            {
                {"task", "sel_selection"},
                {"action", "perform"},
                {"button", "post"},
                {"sel_id", "1216"},
                {"export_type", "1"},
                {"usr_cse_id", "3"},
                {"sort_field[0][field_nm]", ""},
                {"group_field[0]", ""},
                //{"submit", "Uitvoeren"},
            };
            var request = new PostRequest(ResolveUrl("index.php"), postData);
            //var request = new Request(ResolveUrl("index.php?task=sel_selection&action=perform&button=post&sel_id=1216"));
            var response = httpBrowser.DoRequest(request);
            var contents = response.Content;

            if (!contents.StartsWith("\"Lidnummer\","))
            {
                Error(response, "Expected \"Lidnummer\" not found");
            }

            var csvReader = new CsvConverter(contents);
            var membersCsv = new MembersCsv();
            return membersCsv.GetMembers(csvReader);
        }

        private List<Member> GetMemberPage()
        {
            var request = new Request(ResolveUrl("index.php?task=ma_person&action=list"));
            var response = httpBrowser.DoRequest(request);
            return ParseFilterTable(response);
        }

        // TODO maak deze private en voeg Role parameter toe aan de functies die deze nodig hebben.
        public void SwitchRole(Role role)
        {
            var postData = new FormValueCollection
            {
                {"task", "ma_function"},
                {"action", "edit"},
                {"button", "changeRole"},
                {"submit", "Wissel rol"},
                {"role_id", role.Id},
            };
            var request = new PostRequest(ResolveUrl("/index.php"), postData);
            var response = httpBrowser.DoRequest(request);
            CheckNoticeMessage(response, "Rol gewisseld naar");
        }

        public void AddQualification(Member subject, Model.Kwalificatie kwalificatie, DateTime datum, Member examinator)
        {
            var postData = new FormValueCollection
            {
                {"task", "tr_qualification"},
                {"action", "add_post"},
                {"button", ""},
                {"per_id", subject.Lidnummer},
                {"qua_org_id", examinator.Organisatienummer},
                {"cqua_id", kwalificatie.Id},
                {"qua_examinator_id", examinator.Lidnummer},
                {"qua_dt_day", datum.Day},
                {"qua_dt_month", datum.Month},
                {"qua_dt_year", datum.Year},
            };
            var request = new PostRequest(ResolveUrl("/index.php"), postData);
            var response = httpBrowser.DoRequest(request);
            CheckNoticeMessage(response, "Kwalificatie(s) toegekend");
        }

        private void CheckNoticeMessage(Response response, string p)
        {
            var messages = GetNoticeMessages(response.Content);
            foreach (var message in messages)
            {
                if (message.StartsWith(p))
                {
                    return;
                }
            }

            // Error
            var exMsg = string.Format("Expected message \"{0}\" not found", p);
            Error(response, exMsg);            
        }

        private void Error(Response response, string message)
        {
            var filename = SaveResponse(response);
            var exMsg = string.Format("{0}. Response saved to {1}.", message, filename);
            throw new SolResponseException(exMsg);
        }

        private string SaveResponse(Response response)
        {
            var filename = System.IO.Path.GetTempFileName();
            using (var writer = new StreamWriter(filename))
            {
                writer.Write(response.Content);
            }
            return filename;
        }

        // check for <div class="notice_msg">Kwalificatie(s) toegekend</div>
        private IEnumerable<string> GetNoticeMessages(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var messages = document.DocumentNode.SelectNodes("//div[@class='notice_msg']");
            if (messages == null) return Enumerable.Empty<string>();
            return messages.Select(node => node.InnerText);
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

        public List<Member> GetSelection(Role role)
        {
            lock (lockObject)
            {
                SwitchRole(role);
                return GetSelection();
            }
        }
    }
}
