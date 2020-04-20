using Masticore.Infrastructure.Models;
using Microsoft.Azure.Management.WebSites;
using Microsoft.Azure.Management.WebSites.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// Infrastructure service for managing App Plans, EG containers for many Web App instances sitting inside a single Subscription and Region
    /// </summary>
    public class AppPlanInfrastructureService : AzureInfrastructureServiceBase<AppPlan, WebSiteManagementClient, ServerFarmWithRichSku>, IAppPlanInfrastructureService
    {

        /// <summary>
        /// Creates a new instance of WebSiteManagementClient for the current Subscription
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        public static async Task<WebSiteManagementClient> CreateWebsiteClient(Subscription sub)
        {
            var credentials = await AccessToken.AcquireAsync(sub);
            return new WebSiteManagementClient(credentials) { SubscriptionId = sub.SystemId };
        }

        /// <summary>
        /// Create the website client
        /// </summary>
        /// <returns></returns>
        protected override Task<WebSiteManagementClient> CreateClient()
        {
            return CreateWebsiteClient(Subscription);
        }

        /// <summary>
        /// Extracts a ServerFarmWithRichSku for the given AppPlan model 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override ServerFarmWithRichSku CreateOptions(AppPlan model)
        {
            // TODO: Map the AppPlan tier into a specific set of properties for this object - right now it makes an empty S1
            return new ServerFarmWithRichSku(Region.Location);
        }

        /// <summary>
        /// Creates a new instance of an Azure App Plan for the current Subscription and Region
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(AppPlan model)
        {
            Trace.TraceInformation("Creating App Plan for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            var credentials = await AccessToken.AcquireAsync(Subscription);
            using (var websiteClient = new WebSiteManagementClient(credentials) { SubscriptionId = Subscription.SystemId })
            {
                var options = CreateOptions(model);
                var serverFarm = await websiteClient.ServerFarms.CreateOrUpdateServerFarmAsync(Region.Name, model.Name, options);
                model.SystemId = serverFarm.Id;
                Trace.TraceInformation("Created App Plan for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);
            }
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task ResumeAsync(AppPlan model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override Task RetractAsync(AppPlan model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task SuspendAsync(AppPlan model)
        {
            throw new NotImplementedException();
        }
    }
}
