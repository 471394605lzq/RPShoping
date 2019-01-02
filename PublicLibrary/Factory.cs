using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary
{
  public class Factory
    {
        private string ClassName;
        private string UrlBase;
        private Dictionary<string, string> headers;
        public Factory(string ClassName, Dictionary<string, string> headers, string UrlBase)
        {
            this.ClassName = ClassName;
            this.headers = headers;
            this.UrlBase = UrlBase;
        }
        public string Get(string ObjectID)
        {
            string url = UrlBase + "/" + ClassName + "/" + ObjectID;
            return Ajax(url);
        }
        public string Get(string ObjectID, string Relation)
        {
            string url = UrlBase + "/" + ClassName + "/" + ObjectID + "/" + Relation;
            return Ajax(url);
        }
        private string Ajax(string url)
        {
            return Ajax(url, null, null);
        }
        private string Ajax(string url, byte[] data)
        {
            return Ajax(url, data, "POST");
        }
        private string Ajax(string url, byte[] data, string method)
        {
            try
            {
                WebClient webClient = new WebClient();
                foreach (var header in headers)
                {
                    webClient.Headers.Add(header.Key, header.Value);
                }
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
        public string Query()
        {
            string url = UrlBase + "/" + ClassName;
            return Ajax(url);
        }
        public string Query(string Filter)
        {
            string url = UrlBase + "/" + ClassName + "?filter=" + Filter;
            return Ajax(url);
        }
        public string Create(string JsonDataStr)
        {
            string url = UrlBase + "/" + ClassName;

            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(JsonDataStr);
            return Ajax(url, Data);
        }
        public string Create(Object body)
        {
            var obj = handleFile(body);
            string url = UrlBase + "/" + ClassName;

            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data);
        }
        public string Create(string ObjectID, string Relation, string JsonDataStr)
        {
            string url = UrlBase + "/" + ClassName + "/" + ObjectID + "/" + Relation;
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(JsonDataStr);
            return Ajax(url, Data);
        }
        public string Create(string ObjectID, string Relation, Object body)
        {
            var obj = handleFile(body);
            string url = UrlBase + "/" + ClassName + "/" + ObjectID + "/" + Relation;
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data);
        }
        public string Edit(string ObjectID, string JsonDataStr)
        {
            string url = UrlBase + "/" + ClassName + "/" + ObjectID;
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(JsonDataStr);
            return Ajax(url, Data, "PUT");
        }
        public string Edit(string ObjectID, Object body)
        {
            var obj = handleFile(body);
            string url = UrlBase + "/" + ClassName + "/" + ObjectID;
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data, "PUT");
        }
        public string Delete(string ObjectID)
        {
            string url = UrlBase + "/" + ClassName + "/" + ObjectID;
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes("");
            return Ajax(url, Data, "DELETE");
        }
        public string Delete(string ObjectID, string Relation)
        {
            string url = UrlBase + "/" + ClassName + "/" + ObjectID + "/" + Relation;
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes("");
            return Ajax(url, Data, "DELETE");
        }
        public string Count()
        {
            string url = UrlBase + "/" + ClassName + "/count";
            return Ajax(url);
        }
        public string Count(string Filter)
        {
            string url = UrlBase + "/" + ClassName + "/count?filter=" + Filter;
            return Ajax(url);
        }
        public string Count(string ObjectID, string Relation)
        {
            string url = UrlBase + "/" + ClassName + "/" + ObjectID + "/" + Relation + "/count";
            return Ajax(url);
        }
        //public string Count(string ObjectID, string Relation, string Filter)
        //{
        //    string url = UrlBase + "/" + ClassName + "/" + ObjectID + "/" + Relation + "/count?filter=" + Filter;
        //    return Ajax(url);
        //}

        public string Exists(string ObjectID)
        {
            string url = UrlBase + "/" + ClassName + "/" + ObjectID + "/exists";
            return Ajax(url);
        }

        public string FindOne()
        {
            string url = UrlBase + "/" + ClassName + "/findOne";
            return Ajax(url);
        }
        public string FindOne(string Filter)
        {
            string url = UrlBase + "/" + ClassName + "/findOne?filter=" + Filter;
            return Ajax(url);
        }
        public string Login(string username, string password)
        {
            string url = UrlBase + "/" + ClassName + "/login";
            var obj = new JObject();
            obj["username"] = username;
            obj["password"] = password;
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data);
        }

        public string Logout()
        {
            string url = UrlBase + "/" + ClassName + "/logout";
            var JsonDataStr = "{}";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(JsonDataStr);
            return Ajax(url, Data);
        }
        public string Logout(string token)
        {
            headers.Remove("Authorization");
            string url = UrlBase + "/" + ClassName + "/logout";
            var obj = new JObject();
            obj["access_token"] = token;
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(obj.ToString());
            return Ajax(url, Data);
        }

        public string Verify(string email, string language, string username)
        {
            string url = UrlBase + "/" + ClassName + "/verifyEmail";
            var JsonDataStr = "{\"emial\":" + email + ",\"language\":" + language + ",\"username\":" + username + "}";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(JsonDataStr);
            return Ajax(url, Data);
        }

        public string Reset(string email, string language, string username)
        {
            string url = UrlBase + "/" + ClassName + "/resetRequest";
            var JsonDataStr = "{\"emial\":" + email + ",\"language\":" + language + ",\"username\":" + username + "}";
            byte[] Data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(JsonDataStr);
            return Ajax(url, Data);
        }

        public string Upload(FileStream stream)
        {
            return Upload(UrlBase + "/file", stream, null);
        }
        public string Upload(FileStream stream, Dictionary<string, string> map)
        {
            return Upload(UrlBase + "/file", stream, map);
        }
        public string Upload(string ObjectID, string Relation, FileStream stream)
        {
            var url = UrlBase + "/" + ClassName + "/" + ObjectID + "/" + Relation;
            return Upload(url, stream, null);
        }
        public string Upload(string ObjectID, string Relation, FileStream stream, Dictionary<string, string> map)
        {
            var url = UrlBase + "/" + ClassName + "/" + ObjectID + "/" + Relation;
            return Upload(url, stream, map);
        }

        private string Upload(string url, FileStream stream, Dictionary<string, string> map)
        {
            var file = new UploadFile
            {
                Name = "file",
                Filename = Path.GetFileName(stream.Name),
                ContentType = "image/png",
                Stream = stream
            };
            if (map == null) map = new Dictionary<string, string>();
            map.Add("filename", Path.GetFileName(stream.Name));
            byte[] result = UploadFiles(url, file, map);
            string ResultStr = System.Text.Encoding.GetEncoding("utf-8").GetString(result);//解码  
            return ResultStr;
        }

        private byte[] UploadFiles(string address, UploadFile file, Dictionary<string, string> values)
        {
            var request = WebRequest.Create(address);
            request.Method = "POST";
            foreach (var header in headers)
            {
                if (header.Key == "Content-type") continue;
                request.Headers.Add(header.Key, header.Value);
            }
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    var kbuffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(kbuffer, 0, kbuffer.Length);
                    kbuffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(kbuffer, 0, kbuffer.Length);
                    kbuffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(kbuffer, 0, kbuffer.Length);
                }

                var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                file.Stream.CopyTo(requestStream);
                buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new MemoryStream())
            {
                responseStream.CopyTo(stream);
                return stream.ToArray();
            }
        }


        private JObject handleFile(Object o)
        {
            var obj = new JObject();
            string[] propertyNames = o.GetType().GetProperties().Select(p => p.Name).ToArray();
            foreach (var prop in propertyNames)
            {
                var p = o.GetType().GetProperty(prop);
                object propValue = p.GetValue(o, null);
                if (propValue is FileStream)
                {
                    var ret = Upload(propValue as FileStream);
                    var file = new JObject();
                    var retfile = JObject.Parse(ret);
                    file["id"] = retfile["id"];
                    file["name"] = retfile["name"];
                    file["url"] = retfile["url"];
                    obj[prop] = file.ToString();
                }
                else
                {
                    obj[prop] = JToken.FromObject(propValue);
                }
            }
            return obj;
        }
    }
}
