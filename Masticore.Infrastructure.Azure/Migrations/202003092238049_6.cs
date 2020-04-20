namespace Masticore.Infrastructure.Azure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Region", "DisplayName", c => c.String());
            AddColumn("dbo.Region", "Domain", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Region", "Domain");
            DropColumn("dbo.Region", "DisplayName");
        }
    }
}
