namespace PortfolioSite.ViewModels
{
    public class CartDetail
    {
        public int RecordId { get; set; }
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Picture { get; set; }
        public decimal SubTotal { get; set; }
        public int Quantity { get; set; }
        public int QuantityInStock { get; set; }
        public decimal Total { get; set; }
        public bool InventoryFlag { get; set; }
    }
}