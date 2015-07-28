using PortfolioSite.Models;
using System.Data.Entity;

namespace PortfolioSite.DataContexts
{
    public class WebStoreDataEntities : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}