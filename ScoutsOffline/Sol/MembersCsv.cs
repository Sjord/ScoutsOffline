using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Sol
{
    class MembersCsv
    {
        public List<Member> GetMembers(CsvConverter csvReader)
        {
            List<Member> members = new List<Member>();
            var keys = csvReader.GetKeys();
            while (true)
            {
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
    }
}
