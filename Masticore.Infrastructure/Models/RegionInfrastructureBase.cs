using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// Base class for infrastructure tied to a region
    /// </summary>
    [DataContract]
    public abstract class RegionInfrastructureBase : InfrastructureBase
    {
        // To-One on Region
        [ForeignKey(nameof(Region))]
        [DataMember]
        [Merge]
        public int? RegionId { get; set; }
        [DataMember]
        public virtual Region Region { get; set; }
    }
}
