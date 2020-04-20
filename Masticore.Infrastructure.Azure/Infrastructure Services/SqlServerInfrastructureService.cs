using Microsoft.Azure;
using Microsoft.Azure.Management.Sql;
using Microsoft.Azure.Management.Sql.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// ISqlServerInfrastructureService Service for managing SQL server instances
    /// </summary>
    public class SqlServerInfrastructureService : InfrastructureServiceBase<SqlServer>, ISqlServerInfrastructureService
    {
        /// <summary>
        /// The desired SQL version
        /// </summary>
        private const string SqlServerVersion = "12.0";

        /// <summary>
        /// The default start IP Address
        /// </summary>
        private const string StartIPAddress = "0.0.0.0";

        /// <summary>
        /// The default end IP Address
        /// </summary>
        private const string EndIPAddress = "0.0.0.0";

        /// <summary>
        /// The default firewall rule name
        /// </summary>
        private const string RuleName = "AllowAllWindowsAzureIps";

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

        /// <summary>
        /// Converts the given model into the API parameters object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected ServerCreateOrUpdateParameters CreateOptions(SqlServer model)
        {
            
            var props = new ServerCreateOrUpdateProperties { AdministratorLogin = model.Username, AdministratorLoginPassword = model.Secret, Version = SqlServerVersion };
            return new ServerCreateOrUpdateParameters { Location = Region.Location, Properties = props };
        }

        /// <summary>
        /// Converts the given model into the API parameters for the server firewall
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected FirewallRuleCreateOrUpdateParameters CreateFirewallOptions(SqlServer model)
        {
            var props = new FirewallRuleCreateOrUpdateProperties { StartIpAddress = StartIPAddress, EndIpAddress = EndIPAddress };
            return new FirewallRuleCreateOrUpdateParameters { Properties = props };
        }

        protected async Task Test(SqlServer model, FirewallRule r)
        {
            using (var client = await CreateClient())
            {
                var options = CreateFirewallOptions(model);
                var response = await client.FirewallRules.CreateOrUpdateAsync(Region.Name, model.Name, RuleName, options);
            }
        }
        /// <summary>
        /// Asychronously creates a new SQL server in Azure for the current subscription
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(SqlServer model)
        {
            Trace.TraceInformation("Creating SQL Server for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var client = await CreateClient())
            {
                var options = CreateOptions(model);
                var response = await client.Servers.CreateOrUpdateAsync(Region.Name, model.Name, options);
                model.SystemId = response.Server.Id;

                Trace.TraceInformation("Created  SQL Server for subscription '{0}' with name '{1}' and id '{2}'", Subscription.SystemId, model.Name, model.SystemId);

                //As part of the deployment, create the default firewall rules that allow Azure connections
                Trace.TraceInformation("Creating default firewall rules for SQL Server '{0}'", model.Name);

                var firewallOptions = CreateFirewallOptions(model);
                var firewallResponse = await client.FirewallRules.CreateOrUpdateAsync(Region.Name, model.Name, RuleName, firewallOptions);

                Trace.TraceInformation("Created default firewall rule '{0}' for SQL Server '{1}'", RuleName, model.Name);

            }
        }

        /// <summary>
        /// Asychronously deletes the SQL server in Azure corresponding to the given model
        /// WARNING: This will delete all contain SQL pools and databases; however, at this time it will NOT automatically delete any persistent objects from the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task RetractAsync(SqlServer model)
        {
            Trace.TraceInformation("Deleting SQL Server for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var client = await CreateClient())
            {
                var options = CreateOptions(model);
                await client.Servers.DeleteAsync(Region.Name, model.Name);
                model.SystemId = null;

                Trace.TraceInformation("Deleted  SQL Server for subscription '{0}' with name '{1}' and id '{2}'", Subscription.SystemId, model.Name, model.SystemId);
            }
        }
    }
}
