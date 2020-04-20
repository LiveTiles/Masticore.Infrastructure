using Masticore.Entity;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System;
using Masticore.Infrastructure.Models;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// ISubscriptionService over EntityFramework
    /// </summary>
    public class SubscriptionEntityService : EntityMergeCrud<Subscription, AzureInfrastructureDbContext>, ISubscriptionService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        public SubscriptionEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// Reads the default subscription
        /// TODO: This should probably be runtime configurable in some way, right now it just returns the first or default model
        /// </summary>
        /// <returns></returns>
        public Task<Subscription> ReadCurrentAsync()
        {
            var currentClientId = ActiveDirectoryAppSettings.ClientId;
            return DbSet.Where(s => s.ClientId == currentClientId).FirstOrDefaultAsync();
        }
    }

    /// <summary>
    /// IRegionService over EntityFramework
    /// </summary>
    public class RegionEntityService : EntityMergeCrud<Region, AzureInfrastructureDbContext>, IRegionService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        public RegionEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// Reads the Region by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Region> ReadByNameAsync(string name)
        {
            return DbSet.Where(r => r.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }
    }

    /// <summary>
    /// ITenantService over EntityFramework
    /// </summary>
    public class TenantEntityService : EntityMergeCrud<Tenant, AzureInfrastructureDbContext>, ITenantService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        public TenantEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// Reads Tenant by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Tenant> ReadByNameAsync(string name)
        {
            return DbSet.Where(t => t.Name == name).FirstOrDefaultAsync();
        }
    }

    /// <summary>
    /// ITenantAliasService over EntityFramework
    /// </summary>
    public class TenantAliasEntityService : EntityMergeCrud<TenantAlias, AzureInfrastructureDbContext>, ITenantAliasService
    {
        protected ITenantService TenantService { get; set; }

        /// <summary>
        /// DI Constructor
        /// </summary>
        public TenantAliasEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider, ITenantService tenantService) : base(provider, false)
        {
            TenantService = tenantService;

        }

        /// <summary>
        /// Reads TenantAlias by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<TenantAlias> ReadByNameAsync(string name)
        {
            return DbSet.Where(t => t.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Async creates a tenant alias, always forcing the same region as the TenantId on the model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<TenantAlias> CreateAsync(TenantAlias model)
        {
            // Fix the alias to have the same region as the Tenant
            var tenant = await TenantService.ReadAsync(model.TenantId);

            // Throw an exception if the tenant cannot be found
            if (tenant == null)
                throw new ArgumentOutOfRangeException(nameof(model.TenantId));

            // Fix region to the tenant's
            model.RegionId = tenant.RegionId;

            // Create normally and return
            return await base.CreateAsync(model);
        }
    }

    /// <summary>
    /// IAppPlanService over EntityFramework
    /// </summary>
    public class AppPlanEntityService : EntityMergeCrud<AppPlan, AzureInfrastructureDbContext>, IAppPlanService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="context"></param>
        public AppPlanEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// Read single app by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<AppPlan> ReadByNameAsync(string name)
        {
            return DbSet.Where(ap => ap.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns the best available AppPlan
        /// By default, this reads the first one available; this should be customized
        /// </summary>
        /// <returns></returns>
        public virtual Task<AppPlan> ReadBestAvailableAsync()
        {
            return DbSet.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns an array of all the app plans
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public Task<AppPlan[]> ReadAllByRegionAsync(int regionId)
        {
            return DbSet.Where(ap => ap.RegionId == regionId).ToArrayAsync();
        }
    }

    /// <summary>
    /// IAppService over EntityFramework
    /// </summary>
    public class AppEntityService : EntityMergeCrud<App, AzureInfrastructureDbContext>, IAppService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="context"></param>
        public AppEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// Returns an array of all apps in the given region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public Task<App[]> ReadAllByRegionAsync(int regionId)
        {
            return DbSet.Where(a => a.RegionId == regionId).ToArrayAsync();
        }

        /// <summary>
        /// Read single App by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<App> ReadByNameAsync(string name)
        {
            return DbSet.Where(ap => ap.Name == name).FirstOrDefaultAsync();
        }
    }

    /// <summary>
    /// IStorageAccountService over EntityFramework
    /// </summary>
    public class StorageAccountEntityService : EntityMergeCrud<StorageAccount, AzureInfrastructureDbContext>, IStorageAccountService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="context"></param>
        public StorageAccountEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// Read single by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<StorageAccount> ReadByNameAsync(string name)
        {
            return DbSet.Where(ap => ap.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Reads best available
        /// This should be customized per the application requirements
        /// </summary>
        /// <returns></returns>
        public virtual Task<StorageAccount> ReadBestAvailableAsync()
        {
            return DbSet.FirstOrDefaultAsync();
        }


        /// <summary>
        /// Reads best available per region
        /// This should be customized per the application requirements
        /// </summary>
        /// <param name="regionId">The id of the <see cref="Region"/></param>
        /// <returns>The best storage account per region</returns>
        public virtual Task<StorageAccount> ReadBestAvailableByRegionAsync(int regionId)
        {
            return DbSet.Where(sa => sa.RegionId == regionId).FirstOrDefaultAsync();
        }



        /// <summary>
        /// Async returns all storage account records for the given region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public Task<StorageAccount[]> ReadAllByRegionAsync(int regionId)
        {
            return DbSet.Where(sa => sa.RegionId == regionId).ToArrayAsync();
        }
    }

    /// <summary>
    /// IStorageContainerService over EntityFramework
    /// </summary>
    public class StorageContainerEntityService : EntityMergeCrud<StorageContainer, AzureInfrastructureDbContext>, IStorageContainerService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="context"></param>
        public StorageContainerEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public Task<StorageContainer[]> ReadAllByTenantAsync(int tenantId)
        {
            return DbSet.Where(sc => sc.TenantId == tenantId).Include(sc => sc.StorageAccount).ToArrayAsync();
        }

        /// <summary>
        /// Read single by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<StorageContainer> ReadByNameAsync(string name)
        {
            return DbSet.Where(ap => ap.Name == name).FirstOrDefaultAsync();
        }
    }

    /// <summary>
    /// ISqlServerService over EntityFramework
    /// </summary>
    public class SqlServerEntityService : EntityMergeCrud<SqlServer, AzureInfrastructureDbContext>, ISqlServerService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="context"></param>
        public SqlServerEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// Reads single by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<SqlServer> ReadByNameAsync(string name)
        {
            return DbSet.Where(ap => ap.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Reads best available SQL Server
        /// This should be customized per the application requirements
        /// </summary>
        /// <returns></returns>
        public Task<SqlServer> ReadBestAvailableAsync()
        {
            return DbSet.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Reads best available SQL Server by region
        /// This should be customized per the application requirements
        /// </summary>
        /// <param name="regionId">The region </param>
        /// <returns></returns>
        public Task<SqlServer> ReadBestAvailableByRegionAsync(int regionId)
        {
            return DbSet.Where(ser => ser.RegionId == regionId && ser.SqlDbs.Count() < 0.95 * ser.MaximumDbCount).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Reads all of the SQL Server records relevant to a given Region ID
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public Task<SqlServer[]> ReadAllByRegionAsync(int regionId)
        {
            return DbSet.Where(s => s.RegionId == regionId).ToArrayAsync();
        }
    }

    /// <summary>
    /// ISqlPoolService over EntityFramework
    /// </summary>
    public class SqlPoolEntityService : EntityMergeCrud<SqlPool, AzureInfrastructureDbContext>, ISqlPoolService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        public SqlPoolEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        /// <summary>
        /// Read single by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<SqlPool> ReadByNameAsync(string name)
        {
            return DbSet.Where(sp => sp.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Reads the best available SQL pool
        /// This should be customized per the application requirements
        /// </summary>
        /// <returns></returns>
        public virtual Task<SqlPool> ReadBestAvailableAsync()
        {
            return DbSet.FirstOrDefaultAsync();
        }


        /// <summary>
        /// Reads the best available SQL pool by region
        /// This should be customized per the application requirements
        /// </summary>
        /// <param name="regionId">The <see cref="Region"/> </param>
        /// <param name="sqlServerId">The <see cref="SqlServer"/> </param>
        /// <returns></returns>
        public virtual Task<SqlPool> ReadBestAvailableByRegionAsync(int regionId, int sqlServerId)
        {
        
            return DbSet.Where(sp => sp.RegionId == regionId && sp.SqlServerId == sqlServerId &&  sp.SqlDbs.Count() < 0.95 * sp.MaximumDbCount).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Returns an array of all SQL Pools in the given region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public Task<SqlPool[]> ReadAllByRegionAsync(int regionId)
        {
            return DbSet.Where(sp => sp.RegionId == regionId).ToArrayAsync();
        }
    }

    /// <summary>
    /// ISqlDbService over EntityFramework
    /// </summary>
    public class SqlDbEntityService : EntityMergeCrud<SqlDb, AzureInfrastructureDbContext>, ISqlDbService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        public SqlDbEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }

        public Task<SqlDb[]> ReadAllByTenantAsync(int tenantId)
        {
            return DbSet.Where(db => db.TenantId == tenantId).Include(db => db.SqlPool).Include(db => db.SqlServer).ToArrayAsync();
        }

        /// <summary>
        /// Read single by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<SqlDb> ReadByNameAsync(string name)
        {
            return DbSet.Where(ap => ap.Name == name).Include(db => db.SqlServer).Include(db => db.SqlPool).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Reads the database instance for the tenantID
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public Task<SqlDb> ReadByTenantId(int tenantId)
        {
            return DbSet.Where(db => db.TenantId == tenantId).FirstOrDefaultAsync();
        }
    }

    /// <summary>
    /// IStorageTableService over EntityFramework
    /// </summary>
    public class StorageTableEntityService : EntityMergeCrud<StorageTable, AzureInfrastructureDbContext>, IStorageTableService
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        public StorageTableEntityService(IDbContextProvider<AzureInfrastructureDbContext> provider) : base(provider, false)
        {
        }


        public Task<StorageTable[]> ReadAllByTenantAsync(int tenantId)
        {
            return DbSet.Where(st => st.TenantId == tenantId).Include(sc => sc.StorageAccount).ToArrayAsync();
        }

        public Task<StorageTable> ReadByNameAsync(string name)
        {
            return DbSet.Where(st => st.Name == name).FirstOrDefaultAsync();
        }
    }
}
