using Microsoft.EntityFrameworkCore;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Infrastructure.config
{
    public class appdbcontext : DbContext
    {
        public appdbcontext(DbContextOptions<appdbcontext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> Cart { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(x => x.Role)
                .HasConversion<string>();

            // One row per (user, product) in the cart
            modelBuilder.Entity<CartItem>()
                .HasIndex(c => new { c.UserId, c.ProductId })
                .IsUnique();

            modelBuilder.Entity<Order>(e =>
            {
                e.Property(o => o.Status).HasConversion<string>();
                e.Property(o => o.ShippingMethod).HasConversion<string>();
                e.Property(o => o.PaymentMethod).HasConversion<string>();
                e.HasIndex(o => o.OrderNumber).IsUnique();

                // Don't delete a user's order history if the user row is deleted
                e.HasOne(o => o.User)
                 .WithMany()
                 .HasForeignKey(o => o.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Coupon>(e =>
            {
                e.HasIndex(c => c.Code).IsUnique();

                // The two codes that were hard-coded in CartPage.jsx
                e.HasData(
                    new Coupon
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Code = "SHOPFORGE25",
                        DiscountPercent = 25,
                        IsActive = true
                    },
                    new Coupon
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Code = "SHOPFORGE40",
                        DiscountPercent = 40,
                        IsActive = true
                    });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
