using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Sol
{
    public class Member
    {
        public string Lidnummer { get; set; }
        public string Lidvoornaam { get; set; }
        public string Lidinitialen { get; set; }
        public string Lidtussenvoegsel { get; set; }
        public string Lidachternaam { get; set; }
        public string Lidstraat { get; set; }
        public string Lidhuisnummer { get; set; }
        public string Lidtoevoegselhuisnr { get; set; }
        public string Lidpostcode { get; set; }
        public string Lidplaats { get; set; }
        public string Lidland { get; set; }
        public string Lidmailadres { get; set; }
        public string Lidgeboortedatum { get; set; }
        public string Lidgeslacht { get; set; }
        public string Lidtelefoon { get; set; }
        public string Lidmobiel { get; set; }
        public string Lidoverigetelefoonnummers { get; set; }
        public string Lidmailadresouderverzorger { get; set; }
        public string Lidziektekostenverzekeraar { get; set; }
        public string Lidziektekostenpolisnummer { get; set; }
        public string LidinschrijfdatumSN { get; set; }
        public string Functie { get; set; }
        public string Functiestartdatum { get; set; }
        public string Speleenheidsoort { get; set; }
        public string Speleenheid { get; set; }
        public string Organisatienummer { get; set; }
        public string Organisatiecategorie { get; set; }
        public string Organisatie { get; set; }
        public string Organisatieplaats { get; set; }

        public bool Matches(string searchText)
        {
            foreach (var property in this.GetType().GetProperties()) {
                var value = property.GetValue(this, null) as string;
                if (value != null)
                {
                    if (-1 != value.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
