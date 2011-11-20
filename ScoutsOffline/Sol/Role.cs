using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Sol
{
    public class Role
    {
        public string Id;
        public string Name;

        public override string ToString()
        {
            return Name;
        }

        private List<int> parts
        {
            get { return Id.Split(',').Select(num => int.Parse(num)).ToList(); }
        }

        public int OrganisatieNr
        {
            get
            {
                return parts[3];
            }
        }
    }
}
