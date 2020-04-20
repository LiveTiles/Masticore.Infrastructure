using Masticore.Storage;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
	/// CRUD Service Interface for persisting app plans
	/// </summary>
	public interface IAppPlanService : ICrudWithByName<AppPlan, int>, ISharedInfrastructureService<AppPlan>, IRegionBasedInfrastructureService<AppPlan> { }

	/// <summary>
	/// CRUD Service Interface for persisting apps
	/// </summary>
	public interface IAppService : ICrudWithByName<App, int>, IRegionBasedInfrastructureService<App> { }

	/// <summary>
	/// CRUD Service Interface for persisting Storage Account
	/// </summary>
	public interface IStorageAccountService : ICrudWithByName<StorageAccount, int>, ISharedInfrastructureService<StorageAccount>, IRegionBasedInfrastructureService<StorageAccount>
    {
        Task<StorageAccount> ReadBestAvailableByRegionAsync(int regionId);
    }

	/// <summary>
	/// CRUD Service Interface for persisting Storage Container
	/// </summary>
	public interface IStorageContainerService : ICrudWithByName<StorageContainer, int>
	{
		Task<StorageContainer[]> ReadAllByTenantAsync(int tenantId);
	}

	/// <summary>
	/// CRUD Service Interface for persisting SQL Server
	/// </summary>
	public interface ISqlServerService : ICrudWithByName<SqlServer, int>, ISharedInfrastructureService<SqlServer>, IRegionBasedInfrastructureService<SqlServer>
	{
		Task<SqlServer> ReadBestAvailableByRegionAsync(int regionId);
	}

	/// <summary>
	/// CRUD Service Interface for persisting SQL Pools
	/// </summary>
	public interface ISqlPoolService : ICrudWithByName<SqlPool, int>, ISharedInfrastructureService<SqlPool>, IRegionBasedInfrastructureService<SqlPool>
	{
		Task<SqlPool> ReadBestAvailableByRegionAsync(int regionId, int sqlServerId);
	}

	/// <summary>
	/// CRUD Service Interface for persisting SQL Databases
	/// </summary>
	public interface ISqlDbService : ICrudWithByName<SqlDb, int>
	{
		Task<SqlDb[]> ReadAllByTenantAsync(int tenantId);
	}
	/// <summary>
	/// CRUD Service Interface for persisting SQL Databases
	/// </summary>
	public interface IStorageTableService : ICrudWithByName<StorageTable, int>
	{
		Task<StorageTable[]> ReadAllByTenantAsync(int tenantId);
	}

    /// <summary>
	/// Infrastructure service interface for managing SQL Server
	/// </summary>
	public interface ISqlServerInfrastructureService : ITenantInfrastructureService<SqlServer> { }

	/// <summary>
	/// Infrastructure service interface for managing SQL Pool
	/// </summary>
	public interface ISqlPoolInfrastructureService : ITenantInfrastructureService<SqlPool>
	{
		SqlServer SqlServer { get; set; }
	}

	/// <summary>
	/// Infrastructure service interface for managing SQL Database
	/// </summary>
	public interface ISqlDbInfrastructureService : ITenantInfrastructureService<SqlDb>
	{
		SqlServer SqlServer { get; set; }
		SqlPool SqlPool { get; set; }
	}

	/// <summary>
	/// Infrastructure service interface for managing Storage Accounts
	/// </summary>
	public interface IStorageAccountInfrastructureService : ITenantInfrastructureService<StorageAccount>
	{
		string BlobDefaultServiceVersion { get; set; }
		Task RefreshSecretAsync(StorageAccount storageAccount);


	}

	/// <summary>
	/// Infrastructure Service for managing the Blob Service property settings. 
	/// </summary>
	public interface IBlobServicePropertyInfrastructureService
	{
		StorageAccount StorageAccount { get; set; }

		Task Deploy(ServiceProperties serviceProperties);

		Task<ServiceProperties> Read();
	}

	/// <summary>
	/// Infrastructure service interface for managing Storage Containers
	/// </summary>
	public interface IStorageContainerInfrastructureService : ITenantInfrastructureService<StorageContainer>
	{
		StorageAccount StorageAccount { get; set; }
	}

	/// <summary>
	/// Infrastructure service interface for managing Storage Tables
	/// </summary>
	public interface IStorageTableInfrastructureService : ITenantInfrastructureService<StorageTable>
	{
		StorageAccount StorageAccount { get; set; }
		IStorageTableFactory CloudTableFactory { get; set; }
	}

	/// <summary>
	/// Infrastructure service interface for managing app plans
	/// </summary>
	public interface IAppPlanInfrastructureService : ITenantInfrastructureService<AppPlan>, ISuspendAndResume<AppPlan> { }

	/// <summary>
	/// Infrastructure service interface for managing web apps
	/// </summary>
	public interface IAppInfrastructureService : ITenantInfrastructureService<App>, ISuspendAndResume<App>
	{
		AppPlan AppPlan { get; set; }
	}
}
