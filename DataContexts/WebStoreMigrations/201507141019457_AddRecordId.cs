namespace PortfolioSite.DataContexts.WebStoreMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecordId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "RecordId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "RecordId");
        }
    }
}
