using Masticore.Entity;

namespace Masticore.Infrastructure.Azure
{
    public class AzureInfrastructureDbContextProvider : DbContextProvider<AzureInfrastructureDbContext>
    {
        protected override AzureInfrastructureDbContext CreateContext()
        {
            return new AzureInfrastructureDbContext();
        }
    }
}