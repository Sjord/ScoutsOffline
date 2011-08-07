using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ScoutsOffline.Http
{
    public class PostRequest : Request
    {
        private Dictionary<string, object> postData;
        public PostRequest(string url, Dictionary<string, object> postData) : base(url)
        {
            this.postData = postData;
        }

        public string GetContent() {
            List<string> pairs = new List<string>();
            foreach (var keyvalue in postData)
            {
                var pair = string.Format("{0}={1}", HttpUtility.UrlEncode(keyvalue.Key), HttpUtility.UrlEncode(keyvalue.Value.ToString()));
                pairs.Add(pair);
            }
            return string.Join("&", pairs.ToArray());
        }
    }
}
