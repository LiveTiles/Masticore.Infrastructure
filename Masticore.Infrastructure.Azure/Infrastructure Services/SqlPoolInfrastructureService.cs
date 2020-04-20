using System.Threading.Tasks;

using Microsoft.Azure.Management.Sql;
using Microsoft.Azure.Management.Sql.Models;
using Microsoft.Azure;
using System.Diagnostics;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// ISqlPoolInfrastructureService Infrastructure service for managing elastic SQL pools in Azure
    /// </summary>
    public class SqlPoolInfrastructureService : InfrastructureServiceBase<SqlPool>, ISqlPoolInfrastructureService
    {
        /// <summary>
        /// Gets or sets the current SQL server
        /// </summary>
        public SqlServer SqlServer { get; set; }

        /// <summary>
        /// Creates a SQL management client
        /// </summary>
        /// <returns></returns>
        protected async Task<SqlManagementClient> CreateClient()
        {
            var token = await AccessToken.AcquireTokenAsync(Subscription);
            var credentials = new TokenCloudCredentials(Subscription.SystemId, token.AccessToken);
            return new SqlManagementClient(credentials);
        }

#pragma warning disable RECS0154 // Parameter is never used
        /// <summary>
        /// Converts the SqlPool model into Azure API parameters
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected ElasticPoolCreateOrUpdateParameters CreateOptions(SqlPool model)
#pragma warning restore RECS0154 // Parameter is never used
        {

            var props = new ElasticPoolCreateOrUpdateProperties
            {
                Edition = "Basic",
                Dtu = 200,
                DatabaseDtuMin = 0,
                DatabaseDtuMax = 5,
            };
            return new ElasticPoolCreateOrUpdateParameters { Location = Region.Location, Properties = props };
        }

        /// <summary>
        /// Asynchronously deploys an elastic SQL pool for this given model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(SqlPool model)
        {
            Trace.TraceInformation("Creating SQL Server for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var client = await CreateClient())
            {
                var parameters = CreateOptions(model);
                var response = await client.ElasticPools.CreateOrUpdateAsync(Region.Name, SqlServer.Name, model.Name, parameters);

                model.SystemId = response.ElasticPool.Id;

                Trace.TraceInformation("Created  SQL Server for subscription '{0}' with name '{1}' and id '{2}'", Subscription.SystemId, model.Name, model.SystemId);
            }
        }

        /// <summary>
        /// Asychronously delets the elastic SQL pool for the given model
        /// WARNING: This deletes all contained SQL databases
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task RetractAsync(SqlPool model)
        {
            Trace.TraceInformation("Deleting SQL Pool for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var client = await CreateClient())
            {
                var parameters = CreateOptions(model);
                await client.ElasticPools.DeleteAsync(Region.Name, SqlServer.Name, model.Name);
                model.SystemId = null;

                Trace.TraceInformation("Deleted  SQL Pool for subscription '{0}' with name '{1}' and id '{2}'", Subscription.SystemId, model.Name, model.SystemId);
            }
        }
    }
}
