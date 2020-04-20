using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// Base class for common aspects of Azure infrastructure services
    /// This is appropriate for any Infrastructure Service where it uses the Rest ServiceClient to connect
    /// </summary>
    /// <typeparam name="ModelType"></typeparam>
    public abstract class AzureInfrastructureServiceBase<ModelType, ClientType, CreateParameterType> : InfrastructureServiceBase<ModelType>
        where ModelType : InfrastructureBase
        where ClientType : Microsoft.Rest.ServiceClient<ClientType>
    {
        /// <summary>
        /// Abstract method for creating the client object for this type of infrastructure
        /// </summary>
        /// <returns></returns>
        protected abstract Task<ClientType> CreateClient();

        /// <summary>
        /// Abstract method for translating the Masticore.Infrastructure persistent model into the parameters to the given client
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected abstract CreateParameterType CreateOptions(ModelType model);
    }
}
