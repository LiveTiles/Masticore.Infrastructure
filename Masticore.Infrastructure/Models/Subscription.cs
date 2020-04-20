using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// Information for an account in the hosting system (EG, Azure Subscription)
    /// </summary>
    [Table(nameof(Subscription))]
    public class Subscription : SecretInfrastructureBase
    {
        /// <summary>
        /// Gets or sets a human-readable, friendly identifier for this piece of infrastructure
        /// </summary>
        [Required]
        [MaxLength(256)]
        [DataMember]
        [Merge]
        public new string Name { get; set; }

        /// <summary>
        /// Gets or sets the computer-readable identifier for this subscription, which is system-dependent
        /// </summary>
        [Display(Name = "Client ID")]
        [DataMember]
        [Merge]
        public string ClientId { get; set; }
    }

}
