using Api.Models;
using Api.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Api.Storage
{
    public class AppDbContext :  IdentityDbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems{ get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders{ get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasData(FakeProductGenerator.GenerateProductList());
            builder.HasPostgresEnum<OrderStatus>();
        }
    }
}