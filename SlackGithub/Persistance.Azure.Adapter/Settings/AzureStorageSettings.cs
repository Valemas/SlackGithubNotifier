namespace Persistence.Azure.Adapter.Settings
{
    public class AzureStorageSettings
    {
        public AzureStorageSettings(string azureConnectionString)
        {
            AzureConnectionString = azureConnectionString;
        }

        public string AzureConnectionString { get; set; }
    }
}