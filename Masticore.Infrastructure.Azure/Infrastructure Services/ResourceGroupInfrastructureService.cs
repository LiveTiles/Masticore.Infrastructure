using Masticore.Infrastructure.Models;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// Infrastructure Service for Resource Groups in Azure - Resource Groups are logically mapped to Regions
    /// </summary>
    public class ResourceGroupInfrastructureService : AzureInfrastructureServiceBase<Region, ResourceManagementClient, ResourceGroup>, IRegionInfrastructureService
    {
        /// <summary>
        /// Extracts options for ResourceGroup from the Region
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override ResourceGroup CreateOptions(Region model)
        {
            return new ResourceGroup { Location = model.Location, Name = model.Name };
        }

        /// <summary>
        /// Creates a new resourc client object for the current subscription
        /// </summary>
        /// <returns></returns>
        protected override async Task<ResourceManagementClient> CreateClient()
        {
            var credentials = await AccessToken.AcquireAsync(Subscription);
            return new ResourceManagementClient(credentials) { SubscriptionId = Subscription.SystemId };
        }

        /// <summary>
        /// Deploys a resource group to azure relevant to the given model, updating the model properties
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task DeployAsync(Region model)
        {
            System.Diagnostics.Trace.TraceInformation("Creating Resource Group for subscription '{0}' with name '{1}' in location '{2}'", Subscription.UniversalId, model.Name, model.Location);

            using (var resourceClient = await CreateClient())
            {
                var options = CreateOptions(model);
                var group = await resourceClient.ResourceGroups.CreateOrUpdateAsync(model.Name, options);
                System.Diagnostics.Trace.TraceInformation("Created Resource Group '{0}' with ID '{1}' created for subscription '{0}' with name '{1}' in '{2}'", group.Id, group.Name, Subscription.UniversalId, model.Name, model.Location);
                model.SystemId = group.Id;
            }
        }

        /// <summary>
        /// Not implemented yet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override Task RetractAsync(Region model)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// A Dictionary of nice Azure Location names to the internal region location. TODO: Temporary mapping until we switch to fluent library 

        ///https://github.com/Azure/azure-libraries-for-net/blob/master/src/ResourceManagement/ResourceManager/Region.cs
        /// </summary>
        public Dictionary<string, string> StaticAzureRegions { get; set; } = new Dictionary<string, string>
        {
            {"West US 2", "westus2"},
            {"Central US", "centralus"},
            {"East US", "eastus"},
            {"East US 2", "eastus2"},
            {"North Central US", "northcentralus"},
            {"South Central US", "southcentralus"},
            {"West Central US", "westcentralus"},
            {"Canada Central", "canadacentral"},
            {"Canada East", "canadaeast"},
            {"Brazil South", "brazilsouth"},
            {"Europe North", "northeurope"},
            {"Europe West", "westeurope"},
            {"UK South", "uksouth"},
            {"UK West", "ukwest"},
            {"Asia East", "eastasia"},
            {"Asia SouthEast", "southeastasia"},
            {"Japan East", "japaneast"},
            {"Japan West", "japanwest"},
            {"Australia East", "australiaeast"},
            {"Australia SouthEast", "australiasoutheast"},
            {"Australia Central", "australiacentral"},
            {"Australia Central 2", "australiacentral2"},
            {"India Central", "centralindia"},
            {"India South", "southindia"},
            {"India West", "westindia"},
            {"Korea South", "koreasouth"},
            {"Korea Central", "koreacentral"},
            {"China North", "chinanorth"},
            {"China East", "chinaeast"},
            {"Germany Central", "germanycentral"},
            {"Germany NorthEast", "germanynortheast"},
            {"US Gov Virginia ", "usgovvirginia"},
            {"US Gov Iowa ", "usgoviowa"},
            {"US Gov Arizona ", "usgovarizona"},
            {"US Gov Texas ", "usgovtexas"},
            {"US Gov Dod East ", "usdodeast"},
            {"US Gov Dod Central ", "usdodcentral"}
        };
    }
}
