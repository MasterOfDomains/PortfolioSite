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
    
    public partial class SizeType
    {
        public SizeType()
        {
            this.CategorySizeTypes = new HashSet<CategorySizeType>();
            this.Sizes = new HashSet<Size>();
        }
    
        public int SizeTypeID { get; set; }
        public string Name { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
    
        public virtual ICollection<CategorySizeType> CategorySizeTypes { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
    }
}
