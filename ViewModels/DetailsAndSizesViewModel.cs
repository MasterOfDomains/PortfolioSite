using PortfolioSite.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PortfolioSite.ViewModels
{
    public class DetailsAndSizesViewModel
    {
        public Item Item { get; set; }
        public string SizeString { get; set; }
        public List<SelectListItem> InventoryId { get; set; }
    }
}