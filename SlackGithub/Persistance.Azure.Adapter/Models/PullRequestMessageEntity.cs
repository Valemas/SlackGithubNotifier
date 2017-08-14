using Microsoft.WindowsAzure.Storage.Table;

namespace Persistence.Azure.Adapter.Models
{
    class PullRequestMessageEntity : TableEntity
    {
        public PullRequestMessageEntity(string pullRequestId, string userName)
        {
            PartitionKey = userName;
            RowKey = pullRequestId;
        }

        public PullRequestMessageEntity()
        {
        }

        public int PullRequestId { get; set; }
        public string TimeStamp { get; set; }
        public string UserName { get; set; }
    }
}