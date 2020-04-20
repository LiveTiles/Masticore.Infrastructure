using Microsoft.Azure.Management.Storage;
using Microsoft.Azure.Management.Storage.Models;
using Masticore.Storage;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// IStorageAccountInfrastructureService Infrastructure service manage storage accounts
    /// </summary>
    public class StorageAccountInfrastructureService : AzureInfrastructureServiceBase<StorageAccount, StorageManagementClient, StorageAccountCreateParameters>, IStorageAccountInfrastructureService
    {
		public string BlobDefaultServiceVersion { get; set; }

		/// <summary>
		/// Refreshes the Secret for the givne model, using the API to read the connection key
		/// </summary>
		/// <param name="model"></param>
		/// <param name="storageClient"></param>
		/// <returns></returns>
		private async Task RefreshSecretAsync(StorageAccount model, StorageManagementClient storageClient)
        {
            var keys = await storageClient.StorageAccounts.ListKeysAsync(Region.Name, model.Name);
            model.Secret = keys.Keys[0].Value;

            Trace.TraceInformation("Refreshed Storage Account key for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);
        }

		private CorsProperties SetCorsRules(CorsProperties newProperties)
		{
			newProperties.CorsRules.Add(new Microsoft.WindowsAzure.Storage.Shared.Protocol.CorsRule() { AllowedHeaders = new System.Collections.Generic.List<string>() { "*" }, AllowedMethods = CorsHttpMethods.Get | CorsHttpMethods.Head });
			return newProperties;
		
		}
		/// <summary>
		/// Creates a storage management client object for the current subscription
		/// </summary>
		/// <returns></returns>
		protected override async Task<StorageManagementClient> CreateClient()
        {
            var credentials = await AccessToken.AcquireAsync(Subscription);
           
            return new StorageManagementClient(credentials) { SubscriptionId = Subscription.SystemId };
        }

        /// <summary>
        /// Converts the given storage account model into API parameters
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override StorageAccountCreateParameters CreateOptions(StorageAccount model)
        {
            // TODO: Use model's tier to determine performance parameters here
            var sku = new Sku(SkuName.StandardGRS, SkuTier.Standard);
            return new StorageAccountCreateParameters(sku, Kind.StorageV2, Region.Location);
      
        }

        /// <summary>
        /// Asychronously deploys a new Azure storage account corresponding to the given model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(StorageAccount model)
        {
            Trace.TraceInformation("Creating Storage Account for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var storageClient = await CreateClient())
            {
                // TODO: Map the App tier into a specific set of properties for this object - right now it makes an empty S1
                var options = CreateOptions(model);
                var storageAccount = await storageClient.StorageAccounts.CreateAsync(Region.Name, model.Name, options);
                model.SystemId = storageAccount.Id;

                Trace.TraceInformation("Created Storage Account for subscription '{0}' with name '{1}' and id '{2}'", Subscription.SystemId, model.Name, storageAccount.Id);
             
                await RefreshSecretAsync(model, storageClient);

                //Set up the Blob Service settings
                //TODO: Use the new service interface
                var cloudStorage = CloudStorageAccount.Parse(model.ToConnectionString());
                cloudStorage.SetCorsForAll();
                cloudStorage.SetServiceVersion(BlobDefaultServiceVersion);
            }

        }

        /// <summary>
        /// Asynchronously updates the Secret property of the given StorageAccount
        /// WARNING: This does NOT save changes for the model, it just sets a property
        /// </summary>
        /// <param name="storageAccount"></param>
        /// <returns></returns>
        public async Task RefreshSecretAsync(StorageAccount storageAccount)
        {
            Trace.TraceInformation("Refreshing Storage Account key for subscription '{0}' with name '{1}'", Subscription.SystemId, storageAccount.Name);
            using (var storageClient = await CreateClient())
            {
                await RefreshSecretAsync(storageAccount, storageClient);
            }
        }

        /// <summary>
        /// Asychronously removes the given storage account
        /// WARNING: This will delete all containers and data within the account, but at this time does NOT automatically delete any releated records in the context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task RetractAsync(StorageAccount model)
        {
            // This is probably too dangerous to leave even have in the code, but we may need it later
            Trace.TraceInformation("Deleting Storage Account for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var storageClient = await CreateClient())
            {
                var options = CreateOptions(model);
                await storageClient.StorageAccounts.DeleteAsync(Region.Name, model.Name);
                model.SystemId = null;

                Trace.TraceInformation("Deleted Storage Account for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);
            }
        }
    }
}
