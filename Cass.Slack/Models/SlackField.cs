using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cass.Slack.Models
{
    /// <summary>
    /// Represents an entry in the indented, list section of
    /// an attachment to a message in the Slack Incoming Webhooks API.
    /// </summary>
    public class SlackField
    {
        /// <summary>
        /// The required title text of the field.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The body text of the field.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Indicates whether this field is small enough to be
        /// displayed with other fields on the same row.
        /// </summary>
        [JsonProperty("short")]
        public bool IsShort { get; set; }
    }
}
