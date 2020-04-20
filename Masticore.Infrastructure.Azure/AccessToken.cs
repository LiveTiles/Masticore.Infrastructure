using Masticore.Infrastructure.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure
{
    /// <summary>
    /// Utility methods for accessing Azure Resource Manager
    /// Code snippets from https://azure.microsoft.com/en-us/documentation/articles/resource-manager-net-sdk/
    /// </summary>
    public static class AccessToken
    {
        /// <summary>
        /// Gets a transactional access token for working with the given Client and Tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public static async Task<TokenCredentials> AcquireAsync(string tenantUri, string tenantId, string clientId, string clientSecret)
        {
            var token = await AcquireTokenAsync(tenantUri, tenantId, clientId, clientSecret);
            var credentials = new TokenCredentials(token.AccessToken, token.AccessTokenType);
            return credentials;
        }

        /// <summary>
        /// Acquires the auth token for the given tenant URI, Id, client ID, and client secret
        /// </summary>
        /// <param name="tenantUri"></param>
        /// <param name="tenantId"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public static async Task<AuthenticationResult> AcquireTokenAsync(string tenantUri, string tenantId, string clientId, string clientSecret)
        {
            System.Diagnostics.Trace.TraceInformation("Acquiring access token for tenant '{0}' and application '{1};", tenantId, clientId);
            var adUrl = string.Format("{0}{1}", tenantUri, tenantId);
            var authContext = new AuthenticationContext(adUrl);

            var credential = new ClientCredential(clientId, clientSecret);

            var token = await authContext.AcquireTokenAsync("https://management.azure.com/", credential);
            return token;
        }

        /// <summary>
        /// Acquires auth token for the given subscription as AuthenticationResult, using implicit Azure AD settings from the config
        /// Requires: ActiveDirectoryAppSettings.AADInstance, ActiveDirectoryAppSettings.Domain, and ActiveDirectoryAppSettings.ClientId to be valid
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public static Task<AuthenticationResult> AcquireTokenAsync(Subscription subscription)
        {
            return AcquireTokenAsync(ActiveDirectoryAppSettings.AADInstance, ActiveDirectoryAppSettings.Domain, ActiveDirectoryAppSettings.ClientId, subscription.Secret);
        }

        /// <summary>
        /// Acquires auth token for the given subscription as TokenCredentials, using implicit Azure AD settings from the config
        /// Requires: ActiveDirectoryAppSettings.AADInstance, ActiveDirectoryAppSettings.Domain, and ActiveDirectoryAppSettings.ClientId to be valid
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public static Task<TokenCredentials> AcquireAsync(Subscription subscription)
        {
            return AcquireAsync(ActiveDirectoryAppSettings.AADInstance, ActiveDirectoryAppSettings.Domain, ActiveDirectoryAppSettings.ClientId, subscription.Secret);
        }
    }
}
