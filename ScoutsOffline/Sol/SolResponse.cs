using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoutsOffline.Http;
using HtmlAgilityPack;

namespace ScoutsOffline.Sol
{
    public class SolResponse
    {
        public SolResponse(Http.Response response)
        {
            this.Roles = GetRoles(response);
            this.UserId = GetUserId(this.Roles);
        }

        private string GetUserId(IEnumerable<Role> roles)
        {
            if (roles == null) return null;
            return GetUserId(roles.First());
        }

        private string GetUserId(Role role)
        {
            var roleId = role.Id;
            var parts = roleId.Split(',');
            return parts[1];
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
        
        public List<Role> Roles { get; set; }
        public string UserId { get; set; }
    }
}
