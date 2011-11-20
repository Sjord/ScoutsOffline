using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Model
{
    /// <summary>
    /// Gebruiker specifiek model
    /// </summary>
    public class StoredModel
    {
        public StoredModel()
        {
            this.MemberList = new MemberList();
        }

        public MemberList MemberList { get; set; }
        public RoleList RoleList { get; set; }
        public string UserId { get; set; }
    }
}
