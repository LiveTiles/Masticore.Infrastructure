using Masticore.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masticore.Infrastructure
{
    /// <summary>
    /// Interface for infrastructure service that is shared between tenants, meaning it must implement some algorithm to discern a best available option for current conditions
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface ISharedInfrastructureService<ModelType>
    {
        Task<ModelType> ReadBestAvailableAsync();
    }

    public interface IRegionBasedInfrastructureService<ModelType>
    {
        Task<ModelType[]> ReadAllByRegionAsync(int regionId);
    }

    /// <summary>
    /// CRUD Service for Subscription objects
    /// </summary>
    public interface ISubscriptionService : ICrudService<Subscription, int>
    {
        Task<Subscription> ReadCurrentAsync();
    }

    /// <summary>
    /// CRUD service for persisting Region objects
    /// </summary>
    public interface IRegionService : ICrudWithByName<Region, int> { }

    /// <summary>
    /// CRUD Service for persisting Tenant objects
    /// </summary>
    public interface ITenantService : ICrudWithByName<Tenant, int> { }

    /// <summary>
    /// CRUD Service for persisting TenantAlias objects
    /// </summary>
    public interface ITenantAliasService : ICrudWithByName<TenantAlias, int> { }

    /// <summary>
    /// Service that can manage infrastructure
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface IInfrastructureService<ModelType> : IService
    {
        Task DeployAsync(ModelType model);
        Task RetractAsync(ModelType model);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface ISuspendAndResume<ModelType>
    {
        Task SuspendAsync(ModelType model);
        Task ResumeAsync(ModelType model);
    }

    /// <summary>
    /// Base interface for a service that manages Subscription and Region connected infrastructure
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface IRegionInfrastructureService<ModelType> : IInfrastructureService<ModelType>
    {
        Subscription Subscription { get; set; }
        Region Region { get; set; }
    }

    /// <summary>
    /// Base interface for a service that manages Tenant-specific infrastructure
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public interface ITenantInfrastructureService<ModelType> : IRegionInfrastructureService<ModelType>
        where ModelType : InfrastructureBase
    {
        Tenant Tenant { get; set; }
    }

    /// <summary>
    /// Infrastructure service for managing Regions
    /// </summary>
    public interface IRegionInfrastructureService : ITenantInfrastructureService<Region>
    {
      Dictionary<string, string> StaticAzureRegions { get; set; }
    }
}
