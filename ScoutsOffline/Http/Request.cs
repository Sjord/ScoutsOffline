using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Http
{
    public class Request
    {
        public string Url;
        public Request(string url)
        {
            this.Url = url;
        }
    }
}
