using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// Base class for infrastructure tied to a tenant (and region)
    /// </summary>
    [DataContract]
    public abstract class TenantInfrastructureBase : RegionInfrastructureBase
    {
        // To-One on Tenant
        [ForeignKey(nameof(Tenant))]
        [DataMember]
        [Merge]
        public int? TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
