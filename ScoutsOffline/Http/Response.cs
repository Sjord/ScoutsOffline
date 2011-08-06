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
        private Stream _stream;

        private Response()
        {
        }

        public Response(WebResponse response)
        {
            this.response = response;
            this._stream = response.GetResponseStream();

            using (var stream = this.response.GetResponseStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                Content = reader.ReadToEnd();
            }
        }

        public string Content;

        internal void Save(string path)
        {
            using (var writer = new StreamWriter(path))
                writer.Write(Content);
        }

        internal static Response FromFile(string path)
        {
            var result = new Response();
            using (var reader = new StreamReader(path))
                result.Content = reader.ReadToEnd();
            return result;
        }
    }
}
