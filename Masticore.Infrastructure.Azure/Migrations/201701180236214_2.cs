namespace Masticore.Infrastructure.Azure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SqlPool", "MaximumDbCount", c => c.Int(nullable: false, defaultValue: 500));
            AddColumn("dbo.SqlServer", "MaximumDbCount", c => c.Int(nullable: false, defaultValue: 5000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SqlServer", "MaximumDbCount");
            DropColumn("dbo.SqlPool", "MaximumDbCount");
        }
    }
}
