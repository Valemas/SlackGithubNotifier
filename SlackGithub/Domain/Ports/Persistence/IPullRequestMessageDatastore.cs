using System.Threading.Tasks;

using Domain.Models;

namespace Domain.Ports.Persistence
{
    public interface IPullRequestMessageDatastore
    {
        Task<string> GetPullRequestMessageTimeStamp(int pullRequestId, string userName);
        Task<bool> StorePullRequestMessage(PullRequestMessage message);
        Task<bool> DeletePullRequestMessage(int pullRequestId, string userName);
    }
}