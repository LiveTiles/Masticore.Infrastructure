namespace Masticore.Infrastructure.Models
{
    /// <summary>
    /// Interface for an object that carries a secret value property (EG, a password)
    /// </summary>
    public interface ISecret
    {
        /// <summary>
        /// This should really be encrypted or otherwise protected before serialization into the DB
        /// </summary>
        string ProtectedSecret { get; set; }

        /// <summary>
        /// Gets or sets the unencrypted secret, useful at runtime
        /// </summary>
        string Secret { get; set; }
    }
}
