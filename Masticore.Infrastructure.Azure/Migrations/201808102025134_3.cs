namespace Masticore.Infrastructure.Azure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TenantAlias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        ClientId = c.String(maxLength: 64),
                        ClientSecret = c.String(maxLength: 64),
                        RedirectUrl = c.String(),
                        RegionId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 256),
                        SystemId = c.String(),
                        UniversalId = c.String(maxLength: 32, fixedLength: true, unicode: false),
                        CreatedUtc = c.DateTime(),
                        UpdatedUtc = c.DateTime(),
                        DeletedUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Region", t => t.RegionId)
                .ForeignKey("dbo.Tenant", t => t.TenantId, cascadeDelete: true)
                .Index(t => t.TenantId)
                .Index(t => t.RegionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantAlias", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.TenantAlias", "RegionId", "dbo.Region");
            DropIndex("dbo.TenantAlias", new[] { "RegionId" });
            DropIndex("dbo.TenantAlias", new[] { "TenantId" });
            DropTable("dbo.TenantAlias");
        }
    }
}
