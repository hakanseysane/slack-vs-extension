using Cass.Slack.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cass.Slack
{
    public static class SlackServiceAdapter
    {
        //TODO: modify this to take a TfsChangesetMessage model instance and translate it into a slack message using a translator
        //that we we can generisize the call to slack itself and add good error handling for slack message results in one place.
        //this will allow us to post other slack messages in the future from this same library. Perhaps the TfsChangesetMessage translation
        //belongs in the calling assembly? Not sure.

        /// <summary>
        /// Posts a TFS check-in to a Slack channel using the given parameters.
        /// </summary>
        public static async Task<HttpResponseMessage> PostToSlack(string requestUri, string channelName,
            string userName, string changesetID, int fileChangedCount, string changesetComment, string changesetUrl)
        {
            var message = new SlackMessage
            {
                Channel = channelName,
                Username = "vsbot",
                Text = string.Format("{0} checked in <{1}|changeset {2}>", userName, changesetUrl, changesetID),
                IconEmoji = ":visualstudio:",
                Attachments = new List<SlackAttachment>
                {
                    new SlackAttachment
                    {
                        Fallback = changesetComment,
                        Color = "#68217A",
                        Fields = new List<SlackField>
                        {
                            new SlackField
                            {
                                Title = string.Format("Changed {0} files.", fileChangedCount),
                                Value = string.IsNullOrWhiteSpace(changesetComment) ? "Please fire me!" : changesetComment,
                                IsShort = false
                            }
                        }
                    }
                }
            };

            var postBody = JsonConvert.SerializeObject(message);

            var content = new StringContent(postBody);

            var client = new HttpClient();
            var response = await client.PostAsync(requestUri, content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = "Error posting to Slack:\n" + response.StatusCode +
                    " - " + await response.Content.ReadAsStringAsync();

                throw new WebException(errorMessage);
            }

            return response;
        }
    }
}
