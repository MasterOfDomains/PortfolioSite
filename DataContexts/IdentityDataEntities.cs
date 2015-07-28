using Microsoft.AspNet.Identity.EntityFramework;
using PortfolioSite.Models;

namespace PortfolioSite.DataContexts
{
    public class IdentityDataEntities : IdentityDbContext<ApplicationUser>
    {
        public IdentityDataEntities()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static IdentityDataEntities Create()
        {
            return new IdentityDataEntities();
        }
    }
}