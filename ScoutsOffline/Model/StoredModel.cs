using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Model
{
    public class StoredModel
    {
        public StoredModel()
        {
            this.MemberList = new MemberList();
        }

        public MemberList MemberList { get; set; }
    }
}
