using Masticore.Storage;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Masticore.Infrastructure.Azure
{
	public class BlobServicePropertyInfrastructureService : IBlobServicePropertyInfrastructureService
	{
		public StorageAccount StorageAccount { get; set; }

		public string BlobDefaultServiceVersion { get; set; }

		/// <summary>
		/// The <see cref="CloudStorageAccount"/> associated with the model-type <see cref="StorageAccount"/> 
		/// </summary>
		private CloudStorageAccount CloudStorageAccount
		{
			get
			{
				return CloudStorageAccount.Parse(StorageAccount.ToConnectionString());
			}
		}
		/// <summary>
		/// Publish the changes to the Blob's service properties.  
		/// </summary>
		/// <param name="serviceProperties">The blob's service properties</param>
		/// <returns>N/A</returns>
		public async Task Deploy(ServiceProperties serviceProperties)
		{
			await CloudStorageAccount.SetServiceProperties(serviceProperties);
		}

		/// <summary>
		/// Reads the service properties from the existing blob service. 
		/// </summary>
		/// <returns>The associated <see cref="ServiceProperties"/> of the associated <see cref="CloudStorageAccount"/></returns>
		public async Task<ServiceProperties> Read()
		{
			var blobClient = CloudStorageAccount.CreateCloudBlobClient();
			return await blobClient.GetServicePropertiesAsync();
		}

	    private CorsRule GetDefaultCorsRule()
		{
			return new CorsRule()
			{
				AllowedHeaders = { "*" },
				AllowedMethods = CorsHttpMethods.Get | CorsHttpMethods.Head,
				AllowedOrigins = { "*" },
				MaxAgeInSeconds = 200
			};
		}


		private ServiceProperties GetDefaultServiceProperties()
		{
			var Cors = new CorsProperties();
			Cors.CorsRules.Add(GetDefaultCorsRule()); ;

			return new ServiceProperties()
			{
				Cors = Cors,
				DefaultServiceVersion = BlobDefaultServiceVersion,
				Logging = null,
				HourMetrics = null,
				MinuteMetrics = null,
			};


		}
	}
}
