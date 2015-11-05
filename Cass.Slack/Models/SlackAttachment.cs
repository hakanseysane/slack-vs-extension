using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cass.Slack.Models
{
    /// <summary>
    /// Represents an attachment to a message to be posted to Slack via the Incoming Webhooks API.
    /// </summary>
    public class SlackAttachment
    {
        /// <summary>
        /// Required text summary of the attachment for devices which cannot display richly formatted attachments.
        /// </summary>
        [JsonProperty("fallback")]
        public string Fallback { get; set; }

        /// <summary>
        /// A line of text which precedes the indented, colored Fields section.
        /// </summary>
        [JsonProperty("pretext")]
        public string Pretext { get; set; }

        /// <summary>
        /// The color to use for the indentation line.
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// A list of fields displayed indented in a table with this attachment.
        /// </summary>
        [JsonProperty("fields")]
        public List<SlackField> Fields { get; set; }
    }
}
