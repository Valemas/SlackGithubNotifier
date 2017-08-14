using System;
using System.Threading.Tasks;

using Domain.Models;
using Domain.Ports.Persistence;

using Microsoft.WindowsAzure.Storage.Table;

using Persistence.Azure.Adapter.Mappers;
using Persistence.Azure.Adapter.Models;

namespace Persistence.Azure.Adapter.Datastores
{
    internal class PullRequestMessageDatastore : IPullRequestMessageDatastore
    {
        private const string TABLE_NAME = "PullRequestMessageTable";
        private const int NO_CONTENT_STATUS_CODE = 204;
        private const int CREATED_STATUS_CODE = 201;
        private readonly CloudTable _table;

        public PullRequestMessageDatastore(CloudTableClient client)
        {
            if(client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            _table = client.GetTableReference(TABLE_NAME);
        }

        public async Task<string> GetPullRequestMessageTimeStamp(int pullRequestId, string userName)
        {
            var pullRequestEntity = await GetPullRequestMessage(pullRequestId, userName);

            return pullRequestEntity.TimeStamp;
        }

        public async Task<bool> StorePullRequestMessage(PullRequestMessage message)
        {
            var messageEntity = PullRequestMessageMapper.ToEntity(message);

            var insertOperation = TableOperation.Insert(messageEntity);

            var result = await _table.ExecuteAsync(insertOperation);

            return result.HttpStatusCode == CREATED_STATUS_CODE || result.HttpStatusCode == NO_CONTENT_STATUS_CODE;
        }

        public async Task<bool> DeletePullRequestMessage(int pullRequestId, string userName)
        {
            var success = false;
            var pullRequestEntity = await GetPullRequestMessage(pullRequestId, userName);

            if(pullRequestEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(pullRequestEntity);
                
                var deleteResult = await _table.ExecuteAsync(deleteOperation);
                success = deleteResult.HttpStatusCode == NO_CONTENT_STATUS_CODE;
            }

            return success;
        }

        private async Task<PullRequestMessageEntity> GetPullRequestMessage(int pullRequestId, string userName)
        {
            TableOperation retrieveOperation =
                TableOperation.Retrieve<PullRequestMessageEntity>(userName, pullRequestId.ToString());

            TableResult retrievedResult = await _table.ExecuteAsync(retrieveOperation);

            return (PullRequestMessageEntity)retrievedResult.Result;
        }
    }
}