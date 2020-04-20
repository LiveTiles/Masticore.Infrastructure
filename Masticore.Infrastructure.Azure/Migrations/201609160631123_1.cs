namespace Masticore.Infrastructure.Azure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppPlan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                        SystemId = c.String(),
                        UniversalId = c.String(maxLength: 32, fixedLength: true, unicode: false),
                        CreatedUtc = c.DateTime(),
                        UpdatedUtc = c.DateTime(),
                        DeletedUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.App",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppPlanId = c.Int(nullable: false),
                        Tier = c.Int(nullable: false),
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
                .ForeignKey("dbo.AppPlan", t => t.AppPlanId, cascadeDelete: true)
                .Index(t => t.AppPlanId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.SqlDb",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SqlPoolId = c.Int(),
                        SqlServerId = c.Int(nullable: false),
                        Tier = c.Int(nullable: false),
                        TenantId = c.Int(),
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
                .ForeignKey("dbo.SqlPool", t => t.SqlPoolId)
                .ForeignKey("dbo.SqlServer", t => t.SqlServerId, cascadeDelete: true)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.SqlPoolId)
                .Index(t => t.SqlServerId)
                .Index(t => t.TenantId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.SqlPool",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SqlServerId = c.Int(nullable: false),
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
                .ForeignKey("dbo.SqlServer", t => t.SqlServerId, cascadeDelete: true)
                .Index(t => t.SqlServerId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.SqlServer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        ProtectedSecret = c.String(),
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
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Tenant",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tier = c.Int(nullable: false),
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
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.StorageAccount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProtectedSecret = c.String(),
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
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.StorageContainer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageAccountId = c.Int(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        TenantId = c.Int(),
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
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .ForeignKey("dbo.StorageAccount", t => t.StorageAccountId, cascadeDelete: true)
                .Index(t => t.StorageAccountId)
                .Index(t => t.TenantId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.StorageTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageAccountId = c.Int(nullable: false),
                        TenantId = c.Int(),
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
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .ForeignKey("dbo.StorageAccount", t => t.StorageAccountId, cascadeDelete: true)
                .Index(t => t.StorageAccountId)
                .Index(t => t.TenantId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Subscription",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        ClientId = c.String(),
                        ProtectedSecret = c.String(),
                        SystemId = c.String(),
                        UniversalId = c.String(maxLength: 32, fixedLength: true, unicode: false),
                        CreatedUtc = c.DateTime(),
                        UpdatedUtc = c.DateTime(),
                        DeletedUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StorageTable", "StorageAccountId", "dbo.StorageAccount");
            DropForeignKey("dbo.StorageTable", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.StorageTable", "RegionId", "dbo.Region");
            DropForeignKey("dbo.StorageContainer", "StorageAccountId", "dbo.StorageAccount");
            DropForeignKey("dbo.StorageContainer", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.StorageContainer", "RegionId", "dbo.Region");
            DropForeignKey("dbo.StorageAccount", "RegionId", "dbo.Region");
            DropForeignKey("dbo.SqlDb", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.Tenant", "RegionId", "dbo.Region");
            DropForeignKey("dbo.SqlPool", "SqlServerId", "dbo.SqlServer");
            DropForeignKey("dbo.SqlDb", "SqlServerId", "dbo.SqlServer");
            DropForeignKey("dbo.SqlServer", "RegionId", "dbo.Region");
            DropForeignKey("dbo.SqlDb", "SqlPoolId", "dbo.SqlPool");
            DropForeignKey("dbo.SqlPool", "RegionId", "dbo.Region");
            DropForeignKey("dbo.SqlDb", "RegionId", "dbo.Region");
            DropForeignKey("dbo.App", "AppPlanId", "dbo.AppPlan");
            DropForeignKey("dbo.App", "RegionId", "dbo.Region");
            DropForeignKey("dbo.AppPlan", "RegionId", "dbo.Region");
            DropIndex("dbo.StorageTable", new[] { "RegionId" });
            DropIndex("dbo.StorageTable", new[] { "TenantId" });
            DropIndex("dbo.StorageTable", new[] { "StorageAccountId" });
            DropIndex("dbo.StorageContainer", new[] { "RegionId" });
            DropIndex("dbo.StorageContainer", new[] { "TenantId" });
            DropIndex("dbo.StorageContainer", new[] { "StorageAccountId" });
            DropIndex("dbo.StorageAccount", new[] { "RegionId" });
            DropIndex("dbo.Tenant", new[] { "RegionId" });
            DropIndex("dbo.SqlServer", new[] { "RegionId" });
            DropIndex("dbo.SqlPool", new[] { "RegionId" });
            DropIndex("dbo.SqlPool", new[] { "SqlServerId" });
            DropIndex("dbo.SqlDb", new[] { "RegionId" });
            DropIndex("dbo.SqlDb", new[] { "TenantId" });
            DropIndex("dbo.SqlDb", new[] { "SqlServerId" });
            DropIndex("dbo.SqlDb", new[] { "SqlPoolId" });
            DropIndex("dbo.App", new[] { "RegionId" });
            DropIndex("dbo.App", new[] { "AppPlanId" });
            DropIndex("dbo.AppPlan", new[] { "RegionId" });
            DropTable("dbo.Subscription");
            DropTable("dbo.StorageTable");
            DropTable("dbo.StorageContainer");
            DropTable("dbo.StorageAccount");
            DropTable("dbo.Tenant");
            DropTable("dbo.SqlServer");
            DropTable("dbo.SqlPool");
            DropTable("dbo.SqlDb");
            DropTable("dbo.App");
            DropTable("dbo.Region");
            DropTable("dbo.AppPlan");
        }
    }
}
