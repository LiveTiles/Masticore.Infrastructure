using Masticore.Infrastructure.Models;
using System.Threading.Tasks;

namespace Masticore.Infrastructure
{
    /// <summary>
    /// Base class for common aspects of Azure infrastructure services
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public abstract class InfrastructureServiceBase<ModelType> : ITenantInfrastructureService<ModelType>
        where ModelType : InfrastructureBase
    {
        /// <summary>
        /// Gets or sets the tenant for the scope of this deployment
        /// Optional value, since not all infrastructure has a Tenant
        /// </summary>
        public Tenant Tenant { get; set; }

        /// <summary>
        /// Gets or sets the region for the scope of this deployment
        /// Optional value, since not all infrastructure has a Region
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// Gets or sets the Subscription for the scope of this deployment
        /// </summary>
        public Subscription Subscription { get; set; }

        /// <summary>
        /// Abstract method for creating infrastructure
        /// Child classes must implement their own strategy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract Task DeployAsync(ModelType model);

        /// <summary>
        /// Abstract method for destroying infrastructure
        /// Child classes must implement their own strategy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract Task RetractAsync(ModelType model);
    }
}
