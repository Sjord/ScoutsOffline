using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Http
{
    public class FormValueCollection : List<KeyValuePair<string, object>>
    {
        public void Add(string item, object value)
        {
            Add(new KeyValuePair<string, object>(item, value));
        }

        public void Update(string item, string value)
        {
            RemoveAll(kvp => kvp.Key == item);
            Add(item, value);
        }
    }
}
