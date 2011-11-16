using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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

        public bool Selected { get; set; }

        private IEnumerable<PropertyInfo> GetSearchProperties()
        {
            return this.GetType().GetProperties().Where(p => p.Name.StartsWith("Lid"));
        }

        private string allProperties = null;

        private string ConcatAllProperties()
        {
            StringBuilder result = new StringBuilder();
            foreach (var property in GetSearchProperties())
            {
                var value = property.GetValue(this, null) as string;
                if (value != null)
                {
                    result.Append(value);
                }
            }
            return result.ToString();
        }

        public bool Matches(IEnumerable<string> keywords)
        {
            foreach (var searchText in keywords)
            {
                if (!Matches(searchText))
                {
                    return false;
                }
            }
            return true;
        }

        private bool Matches(string searchText)
        {
            if (allProperties == null)
            {
                allProperties = ConcatAllProperties();
            }
            if (!Contains(allProperties, searchText))
            {
                return false;
            }

            foreach (var property in GetSearchProperties())
            {
                var value = property.GetValue(this, null) as string;
                if (value != null)
                {
                    if (Contains(value, searchText))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool Contains(string haystack, string needle)
        {
            return -1 != haystack.IndexOf(needle, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Lidnummer.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var member = obj as Member;
            if (member == null) return false;
            return this.Lidnummer == member.Lidnummer
                && this.Functie == member.Functie
                && this.Organisatienummer == member.Organisatienummer
                && this.Functiestartdatum == member.Functiestartdatum;
        }
    }
}
