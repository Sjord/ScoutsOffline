using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ScoutsOffline.Model
{
    public class UserModelRepository : Repository<StoredModel>
    {
        public UserModelRepository(string username) : base(GetFilename(username))
        {
        }

        private static string GetFilename(string username)
        {
            var BaseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(BaseDirectory, username + ".xml");
            return file;
        }
    }
}
