using System.ComponentModel.DataAnnotations;

namespace PortfolioSite.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int RecordId { get; set; }
        public int OrderId { get; set; }
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; }

        // Not for persistence
        public virtual bool RetreivedFromInventory { get; set; }
        public virtual int QuantityShort { get; set; }
    }
}