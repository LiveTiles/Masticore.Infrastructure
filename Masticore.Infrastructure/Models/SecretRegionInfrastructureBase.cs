using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// Base class for infrastructure that has a connection secret
    /// </summary>
    [DataContract]
    public abstract class SecretRegionInfrastructureBase : RegionInfrastructureBase, ISecret
    {
        /// <summary>
        /// Gets or sets the protected secret
        /// In a perfect world. this is the HSM (EG, Azure Key Vault) identifier and not the connection string itself
        /// </summary>
        [Editable(false)]
        public string ProtectedSecret { get; set; }

        /// <summary>
        ///  Gets or sets the unprotected version of the secret (EG, unencrypted)
        /// </summary>
        [Editable(true)]
        [NotMapped]
        [Merge]
        public string Secret
        {
            get
            {
                if (ProtectedSecret == null)
                    return null;
                else
                    return ProtectedSecret.Decrypt();
            }
            set
            {
                ProtectedSecret = value.Encrypt();
            }
        }
    }
}
