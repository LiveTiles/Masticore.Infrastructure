using System.Data.Entity;
using Masticore.Infrastructure.Azure.Migrations;

namespace Masticore.Infrastructure.Azure
{
    public static class AzureInfrastructureDbContextInitializer
    {
        public static IDatabaseInitializer<AzureInfrastructureDbContext> Get()
        {
            return new MigrateDatabaseToLatestVersion<AzureInfrastructureDbContext, Configuration>(true);
        }
    }
}
