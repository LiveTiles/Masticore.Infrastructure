using Masticore.Infrastructure.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// Region-specific model for SQL Server, plus its credentials (username and password)
    /// </summary>
    [Table(nameof(SqlServer))]
    public class SqlServer : SecretRegionInfrastructureBase
    {
        /// <summary>
        /// Gets or sets the username for this SQL server's login information
        /// </summary>
        [Required]
        [DataMember]
        [Merge]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the SQL Pools for this server
        /// To-many on SqlPool
        /// </summary>
        [DataMember]
        [InverseProperty(nameof(SqlPool.SqlServer))]
        public virtual ICollection<SqlPool> SqlPools { get; set; }

        /// <summary>
        /// Gets or sets the SQL databases for this server
        /// to-many on SqlDb
        /// </summary>
        [DataMember]
        [InverseProperty(nameof(SqlDb.SqlServer))]
        public virtual ICollection<SqlDb> SqlDbs { get; set; }


        /// <summary>
        /// Gets or sets the maximum count of databases on the server
        /// </summary>
        [DataMember]
        [Merge]
        public virtual int MaximumDbCount { get; set; }
    }

    /// <summary>
    /// Region-specific model for an elastic SQL pool
    /// </summary>
    [Table(nameof(SqlPool))]
    public class SqlPool : RegionInfrastructureBase
    {
        /// <summary>
        /// Gets or sets the SQL Server for this pool
        /// To-one on SqlServer
        /// </summary>
        [ForeignKey(nameof(SqlServer))]
        [Merge]
        public int SqlServerId { get; set; }
        public virtual SqlServer SqlServer { get; set; }

        /// <summary>
        /// Gets or sets the collection of SQL database in this pool
        /// To-many on SqlDb
        /// </summary>
        [DataMember]
        [InverseProperty(nameof(SqlDb.SqlPool))]
        public virtual ICollection<SqlDb> SqlDbs { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of databases in the pool
        /// </summary>
        [DataMember]
        [Merge]
        public virtual int MaximumDbCount { get; set;}
    }

    /// <summary>
    /// Enum for SQL database tiers in the system
    /// </summary>
    public enum SqlDbTier
    {
        Basic
    }

    /// <summary>
    /// Tenant-specific model for a SQL database
    /// </summary>
    [Table(nameof(SqlDb))]
    public class SqlDb : TenantInfrastructureBase
    {
        /// <summary>
        /// Gets or sets the ID of the SQL database pool
        /// To-one on SqlPool
        /// </summary>
        [ForeignKey(nameof(SqlPool))]
        [Merge(AllowUpdate = false)]
        public int? SqlPoolId { get; set; }
        public virtual SqlPool SqlPool { get; set; }

        /// <summary>
        /// Gets or sets the SQL server ID
        /// To-one on SqlServer
        /// </summary>
        [ForeignKey(nameof(SqlServer))]
        [Merge(AllowUpdate = false)]
        public int SqlServerId { get; set; }
        public virtual SqlServer SqlServer { get; set; }

        public SqlDbTier Tier { get; set; }
    }

    /// <summary>
    /// Region-specific record for Storage Account, along with connection key
    /// </summary>
    [Table(nameof(StorageAccount))]
    public class StorageAccount : SecretRegionInfrastructureBase
    {
        /// <summary>
        /// Gets or sets the collection of storage containers in this account
        /// To-many on StorageContainer
        /// </summary>
        [DataMember]
        [InverseProperty(nameof(StorageContainer.StorageAccount))]
        public virtual ICollection<StorageContainer> StorageContainers { get; set; }

        /// <summary>
        /// Gets or sets the collection of storage tables in this account
        /// To-many on StorageTable
        /// </summary>
        [DataMember]
        [InverseProperty(nameof(StorageTable.StorageAccount))]
        public virtual ICollection<StorageTable> StorageTables { get; set; }
    }

    /// <summary>
    /// Tenant-specific model for storage containers inside of a storage account
    /// </summary>
    [Table(nameof(StorageContainer))]
    public class StorageContainer : TenantInfrastructureBase
    {
        /// <summary>
        /// Gets or sets the ID of the Storage Account for this container
        /// To-one on StorageAccount
        /// </summary>
        [ForeignKey(nameof(StorageAccount))]
        [Merge]
        public int StorageAccountId { get; set; }
        public virtual StorageAccount StorageAccount { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if this container should be public
        /// </summary>
        [DataMember]
        [Merge]
        public bool IsPublic { get; set; }
    }

    /// <summary>
    /// Tenant-specific model for storage tables inside of a storage account
    /// </summary>
    [Table(nameof(StorageTable))]
    public class StorageTable : TenantInfrastructureBase
    {
        /// <summary>
        /// Gets or sets the ID of the Storage Account for this container
        /// To-one on StorageAccount
        /// </summary>
        [ForeignKey(nameof(StorageAccount))]
        [Merge]
        public int StorageAccountId { get; set; }
        public virtual StorageAccount StorageAccount { get; set; }
    }

    /// <summary>
    /// Region-specific model for app plans, EG collections of web apps
    /// </summary>
    [Table(nameof(AppPlan))]
    public class AppPlan : RegionInfrastructureBase
    {
        /// <summary>
        /// Gets or sets the collection of web apps in this plan
        /// To-many on App
        /// </summary>
        [DataMember]
        [InverseProperty(nameof(App.AppPlan))]
        public virtual ICollection<App> WebApps { get; set; }
    }

    /// <summary>
    /// Enum for service tier of the app
    /// </summary>
    public enum AppTier
    {
        Basic
    }

    /// <summary>
    /// Region-specific model for the app instance
    /// </summary>
    [Table(nameof(App))]
    public class App : RegionInfrastructureBase
    {
        /// <summary>
        /// Gets or sets the ID for the app plan of this app
        /// To-one on AppPlan
        /// </summary>
        [ForeignKey(nameof(AppPlan))]
        [Merge]
        public int AppPlanId { get; set; }
        public virtual AppPlan AppPlan { get; set; }

        /// <summary>
        /// Gets or sets the tier for this app
        /// </summary>
        [Display(Name = "Compute Tier")]
        [DataMember]
        [Merge]
        public AppTier Tier { get; set; }
    }

    /// <summary>
    /// Interface defining a DbContext ready for Azure Infrastructure
    /// </summary>
    public interface IAzureInfrastructureDbContext : IInfrastructureDbContext
    {
        DbSet<SqlDb> SqlDbs { get; set; }
        DbSet<SqlPool> SqlPools { get; set; }
        DbSet<SqlServer> SqlServers { get; set; }
        DbSet<StorageAccount> StorageAccounts { get; set; }
        DbSet<StorageContainer> StorageContainers { get; set; }
        DbSet<StorageTable> StorageTables { get; set; }
        DbSet<AppPlan> AppPlans { get; set; }
        DbSet<App> Apps { get; set; }
    }
}
