namespace ScoutsOffline.Http
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using HtmlAgilityPack;

    public class Response
    {
        private readonly WebResponse _response;

        private Response()
        {
        }

        public Response(WebResponse response)
        {
            _response = response;

            using (var stream = _response.GetResponseStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                Content = reader.ReadToEnd();
            }
        }

        public string Content;

        public string ResponseUri { get { return _response.ResponseUri.ToString(); } }

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

        public Cookie GetCookie(string cookieName)
        {
            return ((HttpWebResponse)_response).Cookies[cookieName];
        }

        public IEnumerable<HtmlForm> GetForms()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(Content);
            var forms = doc.DocumentNode.SelectNodes("//form");
            return forms.Select(HtmlForm.FromNode);
        }
    }
}
