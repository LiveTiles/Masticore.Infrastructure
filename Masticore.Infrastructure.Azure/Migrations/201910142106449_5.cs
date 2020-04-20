namespace Masticore.Infrastructure.Azure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantAlias", "PostLogoutUrl", c => c.String());
            AddColumn("dbo.TenantAlias", "Domain", c => c.String(maxLength: 256));
            AddColumn("dbo.TenantAlias", "AuthenticationType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantAlias", "AuthenticationType");
            DropColumn("dbo.TenantAlias", "Domain");
            DropColumn("dbo.TenantAlias", "PostLogoutUrl");
        }
    }
}
