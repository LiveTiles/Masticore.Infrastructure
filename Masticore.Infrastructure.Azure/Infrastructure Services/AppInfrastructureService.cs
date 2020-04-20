using Microsoft.Azure.Management.WebSites;
using Microsoft.Azure.Management.WebSites.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// Infrastructure service for Web apps in Azure, EG a single app in IIS ready for some ASP.Net MVC to run inside of it
    /// </summary>
    public class AppInfrastructureService : AzureInfrastructureServiceBase<App, WebSiteManagementClient, Site>, IAppInfrastructureService
    {
        /// <summary>
        /// Gets or sets the plan for this application
        /// </summary>
        public AppPlan AppPlan { get; set; }

        /// <summary>
        /// Extracts the Site object from the given App model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override Site CreateOptions(App model)
        {
            return new Site(Region.Location) { ServerFarmId = AppPlan.SystemId };
        }

        /// <summary>
        /// Creates an instance of the WebSiteManagementClient client using current credentials
        /// </summary>
        /// <returns></returns>
        protected override Task<WebSiteManagementClient> CreateClient()
        {
            return AppPlanInfrastructureService.CreateWebsiteClient(Subscription);
        }

        /// <summary>
        /// Called after successful deployment
        /// </summary>
        /// <param name="websiteClient"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        protected virtual Task AfterDeployAsync(WebSiteManagementClient websiteClient, Site site, App app)
        {
            // By default, do nothing
            return Task.FromResult(0);
        }

        /// <summary>
        /// Asynchronously deploys a new instance of a web app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(App model)
        {
            Trace.TraceInformation("Creating App for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var websiteClient = await CreateClient())
            {
                // TODO: Map the App tier into a specific set of properties for this object - right now it makes an empty S1
                var options = CreateOptions(model);
                var site = await websiteClient.Sites.CreateOrUpdateSiteAsync(Region.Name, model.Name, options);
                model.SystemId = site.Id;
                await AfterDeployAsync(websiteClient, site, model);
                Trace.TraceInformation("Created App for subscription '{0}' with name '{1}' and id '{2}'", Subscription.SystemId, model.Name, site.Id);
            }
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task ResumeAsync(App model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously deletes the web app for the given App model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task RetractAsync(App model)
        {
            Trace.TraceInformation("Deleting App for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);

            using (var websiteClient = await CreateClient())
            {
                await websiteClient.Sites.DeleteSiteAsync(Region.Name, model.Name);
                model.SystemId = null;
                Trace.TraceInformation("Deleted App for subscription '{0}' with name '{1}'", Subscription.SystemId, model.Name);
            }
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task SuspendAsync(App model)
        {
            throw new NotImplementedException();
        }
    }
}
