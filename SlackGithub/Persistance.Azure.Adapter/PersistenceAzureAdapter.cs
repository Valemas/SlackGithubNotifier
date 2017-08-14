using Domain.Ports.Persistence;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

using Persistence.Azure.Adapter.Datastores;
using Persistence.Azure.Adapter.Settings;

using SimpleInjector;

namespace Persistence.Azure.Adapter
{
    public class PersistenceAzureAdapter
    {
        private readonly AzureStorageSettings _azureStorageSettings;

        internal Container AdapterContainer;

        public PersistenceAzureAdapter(string azureStorageSettings)
        {
            AdapterContainer = new Container();
            _azureStorageSettings = new AzureStorageSettings(azureStorageSettings);
        }

        private void Initialize()
        {
            AdapterContainer = new Container();

            AdapterContainer.RegisterSingleton(_azureStorageSettings);
            RegisterAzureStorage();

            AdapterContainer.Register<IPullRequestMessageDatastore, PullRequestMessageDatastore>();
        }

        public void Register(Container container)
        {
            Initialize();

            container.Register(() => AdapterContainer.GetInstance<IPullRequestMessageDatastore>());
        }

        private void RegisterAzureStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_azureStorageSettings.AzureConnectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            AdapterContainer.RegisterSingleton(tableClient);
        }
    }
}