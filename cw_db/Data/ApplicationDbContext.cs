using cw_db.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cw_db.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasOne<Customer>(s=>s.customer)
                .WithMany(g=>g.Orders)
                .HasForeignKey(s=>s.CustomerId);
            
            builder.Entity<Order>()
                .HasOne<Supplier>(s => s.Supplier)
                .WithMany(g => g.Orders)
                .HasForeignKey(s => s.SupplierId);

            builder.Entity<Feedback>()
                .HasOne<Customer>(s => s.customer)
                .WithMany(g => g.feedBacks)
                .HasForeignKey(s => s.CustomerId);




            builder.Entity<Order>()
                .HasMany<Product>(s => s.Products)
                .WithMany(o => o.Orders)
                .UsingEntity(j => j.ToTable("OrderProduct"));

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers{ get; set; }
        public DbSet<Customer> Customers{ get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<Feedback> Feedbacks{ get; set; }
    }
}