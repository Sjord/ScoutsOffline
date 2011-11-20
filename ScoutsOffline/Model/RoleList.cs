using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ScoutsOffline.Sol;

namespace ScoutsOffline.Model
{
    [DataContract]
    public class RoleList : IListContainer<Role>
    {
        [DataMember]
        private List<Role> Rollen;

        public RoleList(List<Role> list)
        {
            this.Rollen = list;
        }
        
        public List<Role> ToList()
        {
            return Rollen;
        }
    }
}
