using E_Commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ShippingType> ShippingTypes { get; set; }

        public DbSet<Product> Products { get; set; }    

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }    

        public DbSet<Company> Companies { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }  

        public DbSet<OrderType> OrderTypes { get; set; }    

        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
