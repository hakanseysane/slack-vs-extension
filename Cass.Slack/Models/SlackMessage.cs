using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cass.Slack.Models
{
    /// <summary>
    /// Represents the payload of an Incoming Webhook message sent to the Slack API.
    /// See https://slack.com/services/new/incoming-webhook for more details.
    /// </summary>
    public class SlackMessage
    {
        /// <summary>
        /// The channel name to post to. Must be the name of a
        /// currently existing channel in Slack. Example: #general
        /// </summary>
        [JsonProperty("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// The username to post under. Doesn't need to be the name of a registered user.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// The body text of the message to post.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// The emoji to use as the user icon for the message. Example: :ghost:
        /// </summary>
        [JsonProperty("icon_emoji")]
        public string IconEmoji { get; set; }

        /// <summary>
        /// The attachments to this Slack message. Allows for richer formatting and organization.
        /// </summary>
        [JsonProperty("attachments")]
        public List<SlackAttachment> Attachments { get; set; }
    }


}
