using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cass.Slack.Models
{
    /// <summary>
    /// Contains the data about a TFS changeset which should be posted to Slack.
    /// </summary>
    public class TfsChangesetMessage
    {
        public string RequestUri { get; set; } 
        public string ChannelName { get; set; }
        public string UserName { get; set; }
        public string ChangesetID { get; set; }
        public int FileChangedCount { get; set; }
        public string ChangesetComment { get; set; }
        public string ChangesetUrl { get; set; }
    }
}
