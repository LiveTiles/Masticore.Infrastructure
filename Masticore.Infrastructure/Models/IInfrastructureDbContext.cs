using System.Data.Entity;

namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// Interface for DbContext class supporting the basic
    /// </summary>
    public interface IInfrastructureDbContext
    {
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<Region> Regions { get; set; }
        DbSet<Tenant> Tenants { get; set; }
        DbSet<TenantAlias> TenantAlias { get; set; }
    }
}
