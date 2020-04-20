using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// IStorageContainerInfrastructureService Infrastructure service managing blob containers in a storage account
    /// </summary>
    public class StorageContainerInfrastructureService : InfrastructureServiceBase<StorageContainer>, IStorageContainerInfrastructureService
    {
        /// <summary>
        /// Gets or sets the current storage account where the containers are deployed or retracted from
        /// </summary>
        public StorageAccount StorageAccount { get; set; }

        /// <summary>
        /// Asychronously deploys a storage container analogous to the given model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(StorageContainer model)
        {
            Trace.TraceInformation("Creating container '{0}' if not exists and setting permission to public {1}", model.Name, model.IsPublic);

            // Setup connection
            var accountClient = CloudStorageAccount.Parse(StorageAccount.ToConnectionString());
            var blobClient = accountClient.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(model.Name);
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = model.IsPublic ? BlobContainerPublicAccessType.Blob : BlobContainerPublicAccessType.Off;

            // Ensure it exists and set permissions on it
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(permissions);

            // Save the system ID
            model.SystemId = container.Uri.ToString();

            Trace.TraceInformation("Created container '{0}'", model.Name);
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task RetractAsync(StorageContainer model)
        {
            Trace.TraceInformation("Deleting container '{0}' if not exists and setting permission to public {1}", model.Name, model.IsPublic);

            var accountClient = CloudStorageAccount.Parse(StorageAccount.ToConnectionString());
            var blobClient = accountClient.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(model.Name);
            await container.DeleteIfExistsAsync();
            model.SystemId = null;

            Trace.TraceInformation("Deleted container '{0}'", model.Name);
        }
    }
}
