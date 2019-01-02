using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary
{
   public class Message
    {
        /// <summary>
        /// 消息内容本身（必填）。
        /// </summary>
        [JsonProperty("msg_content")]
        public string Content { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("extras")]
        public IDictionary Extras { get; set; }
    }
}
