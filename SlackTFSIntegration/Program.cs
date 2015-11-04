using Cass.Slack;
using Cass.Slack.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SlackTFSIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = Cass.Slack.SlackServiceAdapter.PostToSlack(requestUri: "https://hooks.slack.com/services/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",
                channelName: "#random", userName: "Rikki", changesetID: "12345", fileChangedCount: 38, changesetComment: "", changesetUrl: "www.google.com").Result;
            Console.WriteLine(foo);
            Console.Read();
        }
       
    }
}
