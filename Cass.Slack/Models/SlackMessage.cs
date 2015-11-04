using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cass.Slack.Models
{
    public class SlackMessage
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon_emoji")]
        public string IconEmoji { get; set; }

        [JsonProperty("attachments")]
        public List<SlackAttachment> Attachments { get; set; }
    }


}
