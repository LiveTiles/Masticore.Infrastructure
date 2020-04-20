using Masticore.Infrastructure.Models;
using System.Data.Entity;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// IAzureInfrastructureDbContext over InfrastructureDbContextBase (DbContext)
    /// </summary>
    public class AzureInfrastructureDbContext : DbContext, IAzureInfrastructureDbContext
    {
        /// <summary>
        /// Default connection string name for the Infrastructure database
        /// </summary>
        public const string AzureInfrastructureDbContextConnectionString = "AzureInfrastructureDbContextConnection";

        /// <summary>
        /// Constructor
        /// Sets the connection string on this context, based on the config file
        /// </summary>
        public AzureInfrastructureDbContext() : base(AzureInfrastructureDbContextConnectionString) { }

        /// <summary>
        /// Gets or sets the DbSet of Subscriptions
        /// </summary>
        public DbSet<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of Regions
        /// </summary>
        public DbSet<Region> Regions { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of Tenants
        /// </summary>
        public DbSet<Tenant> Tenants { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of Tenant Aliases
        /// </summary>
        public DbSet<TenantAlias> TenantAlias { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of SqlServers
        /// </summary>
        public DbSet<SqlServer> SqlServers { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of SqlPools
        /// </summary>
        public DbSet<SqlPool> SqlPools { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of SqlDbs
        /// </summary>
        public DbSet<SqlDb> SqlDbs { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of StorageAccounts
        /// </summary>
        public DbSet<StorageAccount> StorageAccounts { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of StorageContainers
        /// </summary>
        public DbSet<StorageContainer> StorageContainers { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of StorageTables
        /// </summary>
        public DbSet<StorageTable> StorageTables { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of AppPlans
        /// </summary>
        public DbSet<AppPlan> AppPlans { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of Apps
        /// </summary>
        public DbSet<App> Apps { get; set; }
    }
}
