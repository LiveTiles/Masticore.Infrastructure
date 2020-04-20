using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Masticore.Storage;

namespace Masticore.Infrastructure.Azure.Infrastructure_Services
{
    /// <summary>
    /// IStorageContainerInfrastructureService Infrastructure service managing blob containers in a storage account
    /// </summary>
    public class StorageTableInfrastructureService : InfrastructureServiceBase<StorageTable>, IStorageTableInfrastructureService
    {

        /// <summary>
        /// Gets or sets the current storage account where the containers are deployed or retracted from
        /// </summary>

        public StorageAccount StorageAccount { get; set; }

        public IStorageTableFactory CloudTableFactory { get; set; }

        public IStorageAccountService StorageAccounts { get; set; }
        public StorageTableInfrastructureService(IStorageTableFactory factory, IStorageAccountService storageAccountService)
        {
            CloudTableFactory = factory;
            StorageAccounts = storageAccountService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual string ExtractTableName(StorageTable model)
        {
            return model.Name;
        }

        /// <summary>
        /// Asychronously deploys a storage container analogous to the given model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(StorageTable model)
        {
            var tableName = ExtractTableName(model);
            Trace.TraceInformation("Creating table '{0}' if not exists", tableName);


            // Setup connection
            StorageAccount = await StorageAccounts.ReadAsync(model.StorageAccountId);
            if (StorageAccount == null)
            {
                Trace.TraceError(String.Format("Unable to find storage account {0}", model.StorageAccountId));
                return;
            }

            CloudTableFactory.StorageConnectionString = StorageAccount.ToConnectionString();

            //Create or return table
            var table = await CloudTableFactory.GetTableAsync(tableName);

            Trace.TraceInformation("Created table '{0}'", model.Name);
        }

        /// <summary>
        /// Deletes the table given
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task RetractAsync(StorageTable model)
        {
            var tableName = ExtractTableName(model);
            Trace.TraceInformation("Removing table '{0}' if not exists", tableName);

            // Setup connection
            StorageAccount = await StorageAccounts.ReadAsync(model.StorageAccountId);
            if (StorageAccount == null)
            {
                Trace.TraceError(String.Format("Unable to find storage account {0}", model.StorageAccountId));
                return;
            }

            CloudTableFactory.StorageConnectionString = StorageAccount.ToConnectionString();

            //Remove table, if exists
            await CloudTableFactory.DeleteTableAsync(tableName);

            Trace.TraceInformation("Deleted table '{0}'", model.Name);
        }
    }
}
