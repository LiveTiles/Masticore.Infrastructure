using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// A tenant in the system, within a single region
    /// </summary>
    [Table(nameof(Tenant))]
    public class Tenant : RegionInfrastructureBase
    {
        [Display(Name = "Tenant Tier")]
        [DataMember]
        [Merge]
        public TenantTier Tier { get; set; }

        /// <summary>
        /// Gets or sets the list of aliases for this tenant
        /// This allows the tenant to be found via those aliases
        /// </summary>
        [DataMember]
        [InverseProperty(nameof(TenantAlias.Tenant))]
        public virtual ICollection<TenantAlias> Aliases { get; set; }
    }

}
