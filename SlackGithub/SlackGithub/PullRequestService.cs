using Newtonsoft.Json;
using RestSharp;
using SlackAPI;
using SlackGithub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SlackGithub
{
    public class PullRequestService
    {
        private readonly SlackSocketClient _client;
        private Dictionary<int, string> _pullRequestMessageDictionary;

        public PullRequestService()
        {
            _client = new SlackSocketClient("xoxp-2717897378-20700026134-223559782067-999e9dc76fb2e49d5c47f53f95cbce59");

            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);

            _client.Connect((connected) => {
                clientReady.Set();
            }, () => {
                _client.GetChannelList((clr) => { });
            });
           
            clientReady.Wait();
        }

        public bool SendMessage(PullRequestEvent pullRequestEvent)
        {
            var success = false;
            if(pullRequestEvent.Action == "opened")
            {
                success = SendPullRequestOpenedMessageWithSlackApi(pullRequestEvent);
            }
            else if (pullRequestEvent.Action == "closed" && pullRequestEvent.Merged)
            {

            }
            else if (pullRequestEvent.Action == "reopened")
            {
                success = SendPullRequestOpenedMessageWithSlackApi(pullRequestEvent);
            }

            return success;
        }

        private bool SendPullRequestOpenedMessageWithSlackApi(PullRequestEvent pullRequestEvent)
        {
            var success = false;
            var channel = _client.Channels.Find(x => x.name.Equals("bot_testing"));
            var text = SlackMessageBuilder.BuildText(pullRequestEvent.PullRequest.html_url, pullRequestEvent.Number, pullRequestEvent.PullRequest.Title, pullRequestEvent.PullRequest.User.UserName);

            _client.PostMessage(response =>
                {
                    if(response.ok)
                    {
                        success = true;
                        StoreMessageReference(pullRequestEvent.PullRequestId, response.ts);
                    }
                },
                channel.id,
                text);

            return success;
        }

        private void StoreMessageReference(int pullRequestId, string timeStamp)
        {
            _pullRequestMessageDictionary.Add(pullRequestId, timeStamp);
        }

        private void DeleteMessageReference(int pullRequestId)
        {
            _pullRequestMessageDictionary.Remove(pullRequestId);
        }

        //private bool FindAndUpdatePullRequestMessage(PullRequestEvent pullRequestEvent)
        //{
        //    var messageTitle = SlackMessageBuilder.FormatPrNumberAndTitle(pullRequestEvent.PullRequest.Title, pullRequestEvent.Number);



        //}
    }
}