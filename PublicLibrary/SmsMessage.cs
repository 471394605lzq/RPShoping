using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class SmsMessage
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("delay_time", DefaultValueHandling = DefaultValueHandling.Include)]
        public int DelayTime { get; set; }
    }
}
