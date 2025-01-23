using GreenHiTech.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GreenHiTech.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       

        public DbSet<User> Users { get; set; }
        public DbSet<CartProduct> cartProducts { get; set; }
        
         public object OrderDetails { get; internal set; }

    }
}
