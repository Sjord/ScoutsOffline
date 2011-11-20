using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Model
{
    public class Kwalificatie
    {
        public int Id { get; set; }
        public int Nummer { get; set; }
        public string Omschrijving { get; set; }

        public override string ToString()
        {
            return Omschrijving;
        }
    }
}
