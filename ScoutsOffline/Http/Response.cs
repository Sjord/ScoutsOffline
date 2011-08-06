using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ScoutsOffline.Http
{
    public class Response
    {
        private WebResponse response;

        public Response(WebResponse response)
        {
            this.response = response;
        }

        public Stream GetStream()
        {
            return response.GetResponseStream();
        }

        public string GetContent()
        {
            using (var stream = this.response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
