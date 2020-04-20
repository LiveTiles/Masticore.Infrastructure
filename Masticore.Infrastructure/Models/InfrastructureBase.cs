using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure
{
    /// <summary>
    /// Base class for all infrastructure items in the system
    /// </summary>
    [DataContract]
    public abstract class InfrastructureBase : PersistentUniversalBase<int>
    {
        /// <summary>
        /// Gets or sets a human-readable, friendly identifier for this piece of infrastructure
        /// </summary>
        [Required]
        [MaxLength(256)]
        [DataMember]
        [Merge(AllowUpdate = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SystemID for this object, which is a system-depdendent string for identitying this resource
        /// </summary>
        [Display(Name = "System ID")]
        [DataMember]
        [Merge(AllowUpdate = false)]
        public string SystemId { get; set; }
    }
}
