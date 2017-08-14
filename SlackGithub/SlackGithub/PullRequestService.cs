using System.Threading.Tasks;

using Domain.Models;
using Domain.Ports.Persistence;

using SlackWebApiClient;

namespace SlackGithub.Webservice
{
    public interface IPullRequestService
    {
        Task<bool> SendMessage(PullRequestEvent pullRequestEvent);
    }

    public class PullRequestService : IPullRequestService
    {
        private readonly IPullRequestMessageDatastore _messageDatastore;
        private readonly SlackApi _slackApi;

        public PullRequestService(IPullRequestMessageDatastore messageDatastore, SlackApi slackApi)
        {
            _messageDatastore = messageDatastore;
            _slackApi = slackApi;
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

            var text = pullRequestEvent.CreatedText();

            var response = await _slackApi.Chat.PostMessage("bot_testing", text);

            if(response.Ok)
            {
                var pullRequestMessage = new PullRequestMessage(pullRequestEvent.PullRequestId,
                    response.Ts,
                    pullRequestEvent.UserName);
                success = await _messageDatastore.StorePullRequestMessage(pullRequestMessage);
            }

            return success;
        }

        private async Task<bool> UpdatePrMessageAfterMerge(PullRequestEvent pullRequestEvent)
        {
            var success = false;
            var timestamp = await _messageDatastore.GetPullRequestMessageTimeStamp(pullRequestEvent.PullRequestId,
                    pullRequestEvent.UserName);
            
            var updateMessage = pullRequestEvent.MergedText("thomas");
            var response = await _slackApi.Chat.Update("bot_testing", timestamp, updateMessage);

            if(response.Ok)
                success= await _messageDatastore.DeletePullRequestMessage(pullRequestEvent.PullRequestId, pullRequestEvent.UserName);

            return success;
        }
    }
}