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
        private readonly SlackClient _client;
        private readonly Dictionary<int, SlackMessage> _pullRequestMessageDictionary;
        private const string CLIENT_ID = "2717897378.223692818324";
        private const string CLIENT_SECRET = "f8c7134112cbcf917d3efeca3bebcffc";
        private const string AUTH_CODE = "xoxp-2717897378-20700026134-223559782067-999e9dc76fb2e49d5c47f53f95cbce59";

        public PullRequestService()
        {
            //ManualResetEventSlim accesTokenReady = new ManualResetEventSlim(false);
            //AccessTokenResponse accessTokenResponse = null;
            //SlackClient.GetAccessToken(response =>
            //{
            //    accessTokenResponse = response;
            //    accesTokenReady.Set();

            //}, CLIENT_ID, CLIENT_SECRET, "", AUTH_CODE);

            //accesTokenReady.Wait();

            _pullRequestMessageDictionary = new Dictionary<int, SlackMessage>();
            _client = new SlackClient("xoxp-2717897378-20700026134-223559782067-999e9dc76fb2e49d5c47f53f95cbce59");

            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);

            LoginResponse response = null;
            _client.Connect((connected) =>
            {
                response = connected;
                clientReady.Set();
            });
           
            clientReady.Wait();
        }

        public bool SendMessage(PullRequestEvent pullRequestEvent)
        {
            var success = false;
            if(pullRequestEvent.Action == "opened")
            {
                success = SendPullRequestOpenedMessage(pullRequestEvent);
            }
            else if (pullRequestEvent.Action == "closed" && pullRequestEvent.Merged)
            {
                success = UpdatePrMessageAfterMerge(pullRequestEvent);
            }
            else if (pullRequestEvent.Action == "reopened")
            {
                success = SendPullRequestOpenedMessage(pullRequestEvent);
            }

            return success;
        }

        private bool SendPullRequestOpenedMessage(PullRequestEvent pullRequestEvent)
        {
            var success = false;

           
                _client.GetChannelList((clr) => { });
                var channel = _client.Channels.Find(x => x.name.Equals("bot_testing"));
                var text = SlackMessageBuilder.BuildText(pullRequestEvent.PullRequest.html_url,
                    pullRequestEvent.Number,
                    pullRequestEvent.PullRequest.Title,
                    pullRequestEvent.PullRequest.User.UserName);
                var slackMessage = new SlackMessage {MessageText = text};

            ManualResetEventSlim messageSent = new ManualResetEventSlim(false);

            _client.PostMessage(response =>
                    {
                        if(response.ok)
                        {
                            success = true;
                            slackMessage.TimeStamp = response.ts;
                            StoreMessageReference(pullRequestEvent.PullRequestId, slackMessage);
                            messageSent.Set();
                        }
                    },
                    channel.id,
                    text);

            messageSent.Wait();

            return success;
        }

        private bool UpdatePrMessageAfterMerge(PullRequestEvent pullRequestEvent)
        {
            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);
            var success = false;
            _client.Connect(connected => {
                SlackMessage slackMessage;
                success = _pullRequestMessageDictionary.TryGetValue(pullRequestEvent.PullRequestId, out slackMessage);
                var channel = _client.Channels.Find(x => x.name.Equals("bot_testing"));

                if (!success)
                {
                    _client.PostMessage(response =>
                        {
                            if (response.ok)
                            {
                                success = true;
                                slackMessage.TimeStamp = response.ts;
                                StoreMessageReference(pullRequestEvent.PullRequestId, slackMessage);
                            }
                        },
                        channel.id,
                        "PR GOT MERGED :merged:");
                }
                else
                {
                    _client.AddReaction(response =>
                        {
                            if(response.ok)
                            {
                                success = true;
                                DeleteMessageReference(pullRequestEvent.PullRequestId);
                            }
                        },
                        timestamp:slackMessage.TimeStamp,
                        name:"merged",
                        channel:channel.id //naming is off here but channel id is required 
                    );
                }
                clientReady.Set();
            });

            clientReady.Wait();

            return success;
        }

        private void StoreMessageReference(int pullRequestId, SlackMessage slackMessage)
        {
            _pullRequestMessageDictionary.Add(pullRequestId, slackMessage);
        }

        private void DeleteMessageReference(int pullRequestId)
        {
            _pullRequestMessageDictionary.Remove(pullRequestId);
        }

        private void ConnectClient()
        {
            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);

            _client.Connect((connected) => {
                clientReady.Set();
            });

            clientReady.Wait();
        }
        
    }
}