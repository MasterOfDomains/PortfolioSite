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
    
    public partial class CategoryName
    {
        public CategoryName()
        {
            this.Categories = new HashSet<Category>();
        }
    
        public int CategoryNameID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Category> Categories { get; set; }
    }
}
