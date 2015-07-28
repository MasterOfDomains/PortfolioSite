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
    
    public partial class Category
    {
        public Category()
        {
            this.CategorySizeTypes = new HashSet<CategorySizeType>();
            this.Items = new HashSet<Item>();
        }
    
        public int CategoryID { get; set; }
        public bool HasGender { get; set; }
        public Nullable<bool> IsMale { get; set; }
        public int CategoryNameID { get; set; }
        public int AgeGroupID { get; set; }
    
        public virtual AgeGroup AgeGroup { get; set; }
        public virtual CategoryName CategoryName { get; set; }
        public virtual ICollection<CategorySizeType> CategorySizeTypes { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
