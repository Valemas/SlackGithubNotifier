using System.Collections.Generic;
using System.Threading.Tasks;

using SlackGithub.Models;

using SlackWebApiClient;
using SlackWebApiClient.Models;

namespace SlackGithub
{
    public class PullRequestService
    {
        private const string CLIENT_ID = "2717897378.223692818324";
        private const string CLIENT_SECRET = "f8c7134112cbcf917d3efeca3bebcffc";
        private const string AUTH_CODE = "xoxp-2717897378-20700026134-223559782067-999e9dc76fb2e49d5c47f53f95cbce59";
        private readonly Dictionary<int, MessageResponse> _pullRequestMessageDictionary;
        private readonly SlackApi _slackApi;

        public PullRequestService()
        {
            _pullRequestMessageDictionary = new Dictionary<int, MessageResponse>();
            _slackApi = new SlackApi(AUTH_CODE);
        }

        public async Task<bool> SendMessage(PullRequestEvent pullRequestEvent)
        {
            var success = false;
            if(pullRequestEvent.Action == PullRequestAction.Opened)
            {
                success = await SendPullRequestOpenedMessage(pullRequestEvent);
            }
            else if(pullRequestEvent.Action == PullRequestAction.Closed && pullRequestEvent.PullRequest.Merged)
            {
                success = await UpdatePrMessageAfterMerge(pullRequestEvent);
            }
            else if(pullRequestEvent.Action == PullRequestAction.Reopened)
            {
                success = await SendPullRequestOpenedMessage(pullRequestEvent);
            }

            return success;
        }

        private async Task<bool> SendPullRequestOpenedMessage(PullRequestEvent pullRequestEvent)
        {
            var success = false;

            var text = SlackMessageBuilder.BuildText(pullRequestEvent.PullRequest.Html_Url,
                pullRequestEvent.Number,
                pullRequestEvent.PullRequest.Title,
                pullRequestEvent.PullRequest.User.UserName);

            var response = await _slackApi.Chat.PostMessage("bot_testing", text);

            if(response.Ok)
            {
                StoreMessageReference(pullRequestEvent.PullRequestId, response);
                success = true;
            }

            return success;
        }

        private async Task<bool> UpdatePrMessageAfterMerge(PullRequestEvent pullRequestEvent)
        {
            MessageResponse message;
            var success = _pullRequestMessageDictionary.TryGetValue(pullRequestEvent.PullRequestId, out message);
            if(!success)
            {
                return false;
            }
            
            var response = _slackApi.reac

            if(response.Ok)
            {
                StoreMessageReference(pullRequestEvent.PullRequestId, response);
            }
            DeleteMessageReference(pullRequestEvent.PullRequestId);

            return true;
        }

        private void StoreMessageReference(int pullRequestId, MessageResponse slackMessage)
        {
            _pullRequestMessageDictionary.Add(pullRequestId, slackMessage);
        }

        private void DeleteMessageReference(int pullRequestId)
        {
            _pullRequestMessageDictionary.Remove(pullRequestId);
        }
    }
}