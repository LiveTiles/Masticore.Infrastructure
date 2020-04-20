using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Masticore.Infrastructure.Azure.Mocks
{
    // TODO: Finish ensuring we have all Mock services necessary to run the PortalApp on a local developer machine
    public class MockAzureService
    {
        public Task Wait()
        {
            return Task.Delay(3000);
        }
    }

    public class MockStorageContainerService : MockAzureService, IStorageContainerInfrastructureService
    {
        public Region Region { get; set; }
        public StorageAccount StorageAccount { get; set; }
        public Subscription Subscription { get; set; }
        public Tenant Tenant { get; set; }

        public Task DeployAsync(StorageContainer model)
        {
            return Wait();
        }

        public Task ResumeAsync(StorageContainer model)
        {
            return Wait();
        }

        public Task RetractAsync(StorageContainer model)
        {
            return Wait();
        }

        public Task SuspendAsync(StorageContainer model)
        {
            return Wait();
        }
    }

    public class MockSqlDbInfrastructureService : MockAzureService, ISqlDbInfrastructureService
    {
        public Region Region { get; set; }

        public SqlPool SqlPool { get; set; }

        public SqlServer SqlServer { get; set; }

        public Subscription Subscription { get; set; }

        public Tenant Tenant { get; set; }

        public Task DeployAsync(SqlDb model)
        {
            return Wait();
        }

        public Task ResumeAsync(SqlDb model)
        {
            return Wait();
        }

        public Task RetractAsync(SqlDb model)
        {
            return Wait();
        }

        public Task SuspendAsync(SqlDb model)
        {
            return Wait();
        }
    }
}
