using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inventoryApiDotnet.Model;
using Microsoft.EntityFrameworkCore;

namespace inventoryApiDotnet.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Product { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<Sell> Sell { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<InvoiceCounter> InvoiceCounter { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Purchase>()
                .HasMany(p => p.purchaseItems)
                .WithOne(pi => pi.Purchase)
                .HasForeignKey(pi => pi.PurchaseId)               
                .OnDelete(DeleteBehavior.Cascade); // when Purchase deleted, delete items too

            modelBuilder.Entity<Sell>()
                .HasMany(p => p.SellItems)
                .WithOne(pi => pi.Sell)
                .HasForeignKey(pi => pi.InvoiceNo)
                .OnDelete(DeleteBehavior.Cascade); // when Purchase deleted, delete items too
        }
    }
}