using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// A class for holding all extensions related to Azure infrastructure or related models
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns the connection string for the given StorageAccount
        /// </summary>
        /// <param name="storageAccount"></param>
        /// <returns></returns>
        public static string ToConnectionString(this StorageAccount storageAccount)
        {
            // return dev storage for local (dev) accounts
            return storageAccount.Name.StartsWith("lt-local-storage")
                ? "UseDevelopmentStorage=true"
                : $"DefaultEndpointsProtocol=https;AccountName={storageAccount.Name};AccountKey={storageAccount.Secret}";
        }

        /// <summary>
        /// Converts the given combination of SqlServer and SqlDb into a connection string
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static string ToConnectionString(this SqlServer server, SqlDb database)
        {
            var serverName = server.Name;
            var dbName = database.Name;
            if (serverName.Contains("localhost") || serverName.Contains("localdb"))
            {
                var fileSetting = serverName.Contains("localdb") ? $"AttachDbFilename=|DataDirectory|{dbName}.mdf" : "";
                return
                    $"Server={serverName};Initial Catalog={dbName}; Integrated Security=True; MultipleActiveResultSets=True; {fileSetting}";
            }

            var username = server.Username;
            var password = server.Secret;
            return
                $"Server=tcp:{serverName}.database.windows.net,1433;Data Source={serverName}.database.windows.net;Initial Catalog={dbName};Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        }

        /// <summary>
        /// Reads a JSON file from the specified path
        /// </summary>
        /// <param name="pathToJson">The full path to the JSON file</param>
        /// <returns>The JSON file contents</returns>
        public static JObject GetJsonFileContents(this string pathToJson)
        {
            var templatefileContent = new JObject();
            using (var file = File.OpenText(pathToJson))
            {
                using (var reader = new JsonTextReader(file))
                {
                    templatefileContent = (JObject)JToken.ReadFrom(reader);
                    return templatefileContent;
                }
            }
        }

        /// <summary>
        /// Utility method for deploying ARM templates, based on files in the file system
        /// </summary>
        /// <param name="resourceManagementClient"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="deploymentName"></param>
        /// <param name="templateFilePath"></param>
        /// <param name="parameterFilePath"></param>
        /// <returns></returns>
        public static Task<DeploymentExtended> DeployTemplate(this ResourceManagementClient resourceManagementClient, string resourceGroupName, string deploymentName, string templateFilePath, string parameterFilePath)
        {
            var templateFileContents = GetJsonFileContents(templateFilePath);
            var parameterFileContents = GetJsonFileContents(parameterFilePath);
            return DeployTemplate(resourceManagementClient, resourceGroupName, deploymentName, templateFileContents, parameterFileContents);
        }

        /// <summary>
        /// Utilities method for deploying ARM templates, based on JSON loaded in memory
        /// </summary>
        /// <param name="resourceManagementClient"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="deploymentName"></param>
        /// <param name="template"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Task<DeploymentExtended> DeployTemplate(this ResourceManagementClient resourceManagementClient, string resourceGroupName, string deploymentName, JObject template, JObject parameters)
        {
            Console.WriteLine(string.Format("Starting template deployment '{0}' in resource group '{1}'", deploymentName, resourceGroupName));
            var deployment = new Deployment();

            deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = template,
                Parameters = parameters,
            };

            return resourceManagementClient.Deployments.BeginCreateOrUpdateAsync(resourceGroupName, deploymentName, deployment);
        }
    }
}
