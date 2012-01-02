using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Sol
{
    public class SolResponseException : Exception
    {
        public SolResponseException(string msg) : base(msg) { }
    }
}
