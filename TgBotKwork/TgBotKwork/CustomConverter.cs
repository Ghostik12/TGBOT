using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBotKwork
{
    public class CustomConverter
    {
        [JsonProperty("role")]
        public string? Role { get; set; }
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("content")]
        public string? Content { get; set; }
        [JsonProperty("content_type")]
        public string? ContentType { get; set; }
    }
}
