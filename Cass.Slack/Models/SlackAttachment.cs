using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cass.Slack.Models
{
    public class SlackAttachment
    {
        [JsonProperty("fallback")]
        public string Fallback { get; set; }

        [JsonProperty("pretext")]
        public string Pretext { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("fields")]
        public List<SlackField> Fields { get; set; }
    }
}
