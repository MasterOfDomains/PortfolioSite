using System.ComponentModel.DataAnnotations;

namespace PortfolioSite.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }

        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public decimal Price { get; set; } // Store price at time of purchase
        
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }

        //public virtual Purchase Purchase { get; set; }
    }
}