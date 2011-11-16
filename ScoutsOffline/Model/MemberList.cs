using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScoutsOffline.Sol;
using System.Collections;

namespace ScoutsOffline.Model
{
    public class MemberList
    {
        public List<Member> Members { get; set; }

        public MemberList()
        {
            this.Members = new List<Member>();
        }

        public void UpdateWith(List<Member> newMembers)
        {
            // TODO oude leden moeten ook verwijderd worden
            var allMembers = new HashSet<Member>(this.Members);
            allMembers.UnionWith(newMembers);
            this.Members = allMembers.OrderBy(m => m.Lidachternaam).ThenBy(m => m.Lidvoornaam).ToList();
        }

        internal void ToggleSelect(Member member)
        {
            member.Selected = !member.Selected;
        }
    }
}
