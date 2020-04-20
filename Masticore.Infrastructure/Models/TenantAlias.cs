using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// An alias name for a tenant in the system
    /// This uses the Name property as the alias, then the foreign key to Tenant to link back
    /// </summary>
    [Table(nameof(TenantAlias))]
    public class TenantAlias : RegionInfrastructureBase
    {
        // Name property is for Host value

        /// <summary>
        /// Gets or sets the Id of the parent Tenant for this alias
        /// </summary>
        [ForeignKey(nameof(Tenant))]
        [Merge(AllowUpdate = false)]
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }

        [Display(Name = "Client ID")]
        [Merge]
        [MaxLength(64)]
        [DataMember]
        public string ClientId { get; set; }

        [Merge]
        [MaxLength(64)]
        public string ClientSecret { get; set; }

        [Display(Name = "Redirect URL")]
        [Merge]
        [DataMember]
        public string RedirectUrl { get; set; }

        [Display(Name = "Post Logout URL")]
        [Merge]
        [DataMember]
        public string PostLogoutUrl { get; set; }

        [Display(Name = "Tenant Path")]
        [Merge]
        [MaxLength(256)]
        [DataMember]
        public string TenantPath { get; set; }

        [Display(Name = "Profile Policy")]
        [Merge]
        [MaxLength(256)]
        [DataMember]
        public string ProfilePolicy { get; set; }

        [Display(Name = "Sign In Policy")]
        [Merge]
        [MaxLength(256)]
        [DataMember]
        public string SigninPolicy { get; set; }


        [Display(Name = "Sign Up Policy")]
        [Merge]
        [MaxLength(256)]
        [DataMember]
        public string SignupPolicy { get; set; }

        [Display(Name = "Domain")]
        [Merge]
        [MaxLength(256)]
        [DataMember]
        public string Domain { get; set; }

        [Display(Name = "Authentication Type")]
        [Merge]
        [DataMember]
        public AuthenticationType AuthenticationType { get; set; }
    }
}
