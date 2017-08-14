using Domain.Models;

using Persistence.Azure.Adapter.Models;

namespace Persistence.Azure.Adapter.Mappers
{
    internal static class PullRequestMessageMapper
    {
        internal static PullRequestMessageEntity ToEntity(PullRequestMessage pullRequestMessage)
        {
            var entity =
                new PullRequestMessageEntity(pullRequestMessage.PullRequestId.ToString(),
                    pullRequestMessage.UserName) {TimeStamp = pullRequestMessage.TimeStamp};

            return entity;
        }
    }
}