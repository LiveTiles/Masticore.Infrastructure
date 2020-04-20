using System.Web;

namespace Masticore.Infrastructure.Azure
{
    public class TenantNotFoundException : HttpException
    {
        public TenantNotFoundException(string tenantName) : base(404, string.Format("{0} Tenant Not Found", tenantName)) { }
    }
}
