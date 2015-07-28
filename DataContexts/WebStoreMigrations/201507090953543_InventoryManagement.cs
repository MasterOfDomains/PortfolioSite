namespace PortfolioSite.DataContexts.WebStoreMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryManagement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "RetreivedFromInventory", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderDetails", "QuantityShort", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "AllItemsAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "AllItemsRetreived", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "AllItemsRetreived");
            DropColumn("dbo.Orders", "AllItemsAvailable");
            DropColumn("dbo.OrderDetails", "QuantityShort");
            DropColumn("dbo.OrderDetails", "RetreivedFromInventory");
        }
    }
}
