using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace ScoutsOffline.Sol
{
    class FakeScoutsOnLine
    {
        public bool Authenticate(string username, string password)
        {
            return true;
        }

        public void StartGetMembers(ScoutsOnLine.MembersAvailable callback)
        {
            var file = @"C:\Documents and Settings\Sjoerd\Mijn documenten\Visual Studio 2010\Projects\ScoutsOffline\selectie_1216.csv";
            callback(GetMembers(file), 0, 2);
            Thread.Sleep(1000);
            file = @"C:\Documents and Settings\Sjoerd\Mijn documenten\Visual Studio 2010\Projects\ScoutsOffline\selectie_1217.csv";
            callback(GetMembers(file), 1, 2);
        }

        private List<Member> GetMembers(string file)
        {
            using (var reader = new StreamReader(file))
            {
                var contents = reader.ReadToEnd();
                var csvReader = new CsvConverter(contents);
                var membersCsv = new MembersCsv();
                var members = membersCsv.GetMembers(csvReader);
                return members;
            }
        }
    }
}
