//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortfolioSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventory
    {
        public Inventory()
        {
            this.Stocks = new HashSet<Stock>();
        }
    
        public int InventoryID { get; set; }
        public int QuantityInStock { get; set; }
    
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}