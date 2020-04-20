namespace Masticore.Infrastructure.Azure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4PolicyInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantAlias", "TenantPath", c => c.String(maxLength: 256));
            AddColumn("dbo.TenantAlias", "ProfilePolicy", c => c.String(maxLength: 256));
            AddColumn("dbo.TenantAlias", "SigninPolicy", c => c.String(maxLength: 256));
            AddColumn("dbo.TenantAlias", "SignupPolicy", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantAlias", "SignupPolicy");
            DropColumn("dbo.TenantAlias", "SigninPolicy");
            DropColumn("dbo.TenantAlias", "ProfilePolicy");
            DropColumn("dbo.TenantAlias", "TenantPath");
        }
    }
}
