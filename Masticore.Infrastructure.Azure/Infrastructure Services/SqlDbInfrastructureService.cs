using System.Threading.Tasks;

using Microsoft.Azure.Management.Sql;
using Microsoft.Azure.Management.Sql.Models;
using Microsoft.Azure;
using System.Diagnostics;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// Infrastructure service implementation for ISqlDbInfrastructureService
    /// </summary>
    public class SqlDbInfrastructureService : InfrastructureServiceBase<SqlDb>, ISqlDbInfrastructureService
    {
        /// <summary>
        /// Gets or sets the current SQL server for this database
        /// </summary>
        public SqlServer SqlServer { get; set; }

        /// <summary>
        /// Gets or sets the optional current elastic SQL pool
        /// </summary>
        public SqlPool SqlPool { get; set; }

        /// <summary>
        /// Creates a SQL management client instance for the current subscription
        /// </summary>
        /// <returns></returns>
        protected async Task<SqlManagementClient> CreateClient()
        {
            var token = await AccessToken.AcquireTokenAsync(Subscription);
            var credentials = new TokenCloudCredentials(Subscription.SystemId, token.AccessToken);
            return new SqlManagementClient(credentials);
        }

        /// <summary>
        /// Extracts options for the current state
        /// </summary>
        /// <returns></returns>
        protected DatabaseCreateOrUpdateParameters CreateOptions()
        {
            // TODO: Determine optimal elastic pool properties
            if (SqlPool != null)
            {
                var props = new DatabaseCreateOrUpdateProperties { ElasticPoolName = SqlPool.Name };
                return new DatabaseCreateOrUpdateParameters { Location = Region.Location, Properties = props };
            }
            else
            {
                var props = new DatabaseCreateOrUpdateProperties { };
                return new DatabaseCreateOrUpdateParameters { Location = Region.Location, Properties = props };
            }
        }

        /// <summary>
        /// Asynchronously deploys a SQL database in Azure for the current SQL Server and optionally the current Elastic SQL Pool
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(SqlDb model)
        {
            Trace.TraceInformation("Creating SQL Database for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var client = await CreateClient())
            {
                var parameters = CreateOptions();
                var response = await client.Databases.CreateOrUpdateAsync(Region.Name, SqlServer.Name, model.Name, parameters);
                model.SystemId = response.Database.Id;

                Trace.TraceInformation("Created  SQL Database for subscription '{0}' with name '{1}' and id '{2}'", Subscription.SystemId, model.Name, model.SystemId);
            }
        }

        /// <summary>
        /// Asynchronously deletes a SQL Database in Azure for the current SQL Server (Pool does not need to be specified)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task RetractAsync(SqlDb model)
        {
            Trace.TraceInformation("Deleting SQL Database for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var client = await CreateClient())
            {
                var parameters = CreateOptions();
                await client.Databases.DeleteAsync(Region.Name, SqlServer.Name, model.Name);
                model.SystemId = null;

                Trace.TraceInformation("Deleted  SQL Database for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);
            }
        }
    }
}
