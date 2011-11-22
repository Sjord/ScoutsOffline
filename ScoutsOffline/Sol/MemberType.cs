using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using FastReflection;

namespace ScoutsOffline.Sol
{
    class MemberType
    {
        private Type memberType;
        public IEnumerable<FastProperty<Member, string>> SearchProperties { get; private set; }

        public MemberType()
        {
            this.memberType = typeof(Member);
            SearchProperties = GetSearchProperties();
        }

        private IEnumerable<FastProperty<Member, string>> GetSearchProperties()
        {
            return this.memberType.GetProperties().Where(p => p.Name.StartsWith("Lid"))
                .Select(p => p.ToFastProperty<Member, string>()).ToList();
        }
    }
}
