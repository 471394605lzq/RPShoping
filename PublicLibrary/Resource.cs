using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary
{
  public class Resource
    {
        private string AppId;
        private string AppKey;
        private string UrlBase;
        private Dictionary<string, string> headers = new Dictionary<string, string>();
        public Resource(string AppId, string AppKey, string UrlBase = "https://d.apicloud.com/mcm/api")
        {
            this.AppId = AppId;
            this.AppKey = AppKey;
            this.UrlBase = UrlBase;

            headers.Add("X-APICloud-AppId", AppId);
            headers.Add("X-APICloud-AppKey", X_APICloud_AppKey);
            headers.Add("Content-type", "application/json;charset=UTF-8");
        }
        private string X_APICloud_AppKey
        {
            get
            {
                long amp = (long)(DateTime.Now - new DateTime(1970, 01, 01)).TotalMilliseconds;

                String value = String.Format("{0}UZ{1}UZ{2}", AppId, AppKey, amp);

                byte[] buffer = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(value));

                StringBuilder builder = new StringBuilder();

                foreach (byte num in buffer)
                {
                    builder.AppendFormat("{0:x2}", num);
                }
                return builder.ToString() + "." + amp;
            }
        }

        public Factory Factory(string ClassName)
        {
            return new Factory(ClassName, headers, UrlBase);
        }

        public string Batch(string requests)
        {
            var obj = new JObject();
            obj["requests"] = JArray.Parse(requests);
            string url = UrlBase + "/batch";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "POST");
        }
        public string Batch(List<Object> requests)
        {
            var obj = new JObject();
            obj["requests"] = JToken.FromObject(requests);
            string url = UrlBase + "/batch";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "POST");
        }

        public void SetHeader(string key, string value)
        {
            headers.Add(key, value);
        }
        public string Ajax(string url, byte[] data, string method)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("X-APICloud-AppId", AppId);
                webClient.Headers.Add("X-APICloud-AppKey", X_APICloud_AppKey);
                webClient.Headers.Add("Content-type", "application/json;charset=UTF-8");
                string ResponseData;
                if (data != null)
                {
                    var responseData = webClient.UploadData(url, method, data);
                    ResponseData = System.Text.Encoding.GetEncoding("UTF-8").GetString(responseData);
                }
                else
                {
                    ResponseData = webClient.DownloadString(url);
                }

                return ResponseData;
            }
            catch (WebException e)
            {
                return "{ \"Error\":{ \"msg\": \"" + e.Message + "\"}}";
            }
        }
    }
}
