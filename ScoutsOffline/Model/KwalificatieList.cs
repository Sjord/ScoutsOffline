using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ScoutsOffline.Model
{
    [DataContract]
    public class KwalificatieList : IListContainer<Kwalificatie>
    {
        [DataMember]
        private List<Kwalificatie> Kwalificaties;

        public List<Kwalificatie> ToList()
        {
            return Kwalificaties;
        }

        public static KwalificatieList Get() {
            var repo = new Repository<KwalificatieList>("SolData.xml");
            return repo.Get();
        }
    }
}
