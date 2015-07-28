using System.Collections.Generic;

namespace PortfolioSite.ViewModels
{
    public class ShoppingCartViewModel
    {
        public string CartId { get; set; }
        public int NumItems { get; set; }
        public decimal Total { get; set; }
        public string UserName { get; set; }

        public List<CartDetail> Details { get; set; }
    }
}