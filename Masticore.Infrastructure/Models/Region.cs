using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// A single group of infrastructure, most often tied to a geographical region
    /// </summary>
    [Table(nameof(Region))]
    public class Region : InfrastructureBase
    {
        /// <summary>
        /// Gets or sets the location of this infrastructure
        /// This is either a human-readable descriptor of the location
        /// or perhaps a technical necessity
        /// </summary>
        [DataMember]
        [Merge]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the display name for this region
        /// This should be used by the user interface to define this region
        /// </summary>
        [Display(Name = "Display Name")]
        [DataMember]
        [Merge]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the domain associated with this region
        /// </summary>
        [DataMember]
        [Merge]
        public string Domain { get; set; }
    }

}
