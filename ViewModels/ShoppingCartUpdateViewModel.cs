namespace PortfolioSite.ViewModels
{
    public class ShoppingCartUpdateViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public decimal ItemTotal { get; set; }
        public int UpdateId { get; set; }
        public bool Success { get; set; }
    }
}