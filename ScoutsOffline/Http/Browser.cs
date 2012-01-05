

using System.Net;
using System.Text;
namespace ScoutsOffline.Http
{
    public class Browser
    {
        private CookieContainer cookies;

        private int timeout = 10000;

        public Browser()
        {
            cookies = new CookieContainer();
        }

        public Response DoRequest(Request request)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            webRequest.CookieContainer = cookies;
            webRequest.Timeout = this.timeout;

            if (request is PostRequest)
            {
                webRequest.Method = "POST";
                var content = ((PostRequest)request).GetContent();
                var contentBytes = Encoding.ASCII.GetBytes(content);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = contentBytes.Length;

                using (var stream = webRequest.GetRequestStream())
                {
                    stream.Write(contentBytes, 0, contentBytes.Length);
                }
            }
            var webResponse = webRequest.GetResponse();
            return new Response(webResponse);
        }

        public Cookie GetCookie(string url, string cookieName)
        {
            return cookies.GetCookies(new System.Uri(url))[cookieName];
        }
    }
}
