using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventra.Data.Entities;
using Inventra.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Inventra.Data
{
    public class InventraDbContext : IdentityDbContext<InventraUser>
    {
        public InventraDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(od => new { od.OrderId, od.ProductId });

                entity.HasOne(od => od.Order)
                    .WithMany(o => o.OrderDeatails)
                    .HasForeignKey(od => od.OrderId);

                entity.HasOne(od => od.Product)
                    .WithMany(p => p.OrderDeatails)
                    .HasForeignKey(od => od.ProductId);

                entity.Property(od => od.Subtotal)
                    .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<WarehouseLocation> WarehouseLocations { get; set; }
        public DbSet<CourierCountry> CourierCountries { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Country> Countries { get; set; }



    }
}
