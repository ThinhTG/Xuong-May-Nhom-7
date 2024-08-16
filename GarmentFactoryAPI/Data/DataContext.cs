using GarmentFactoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GarmentFactoryAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<AssemblyLine> AssemblyLines { get; set; }
        public DbSet<TaskProduct> TaskProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the relationships
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete is okay here

            modelBuilder.Entity<AssemblyLine>()
        .HasOne(al => al.TaskProduct)
        .WithMany(tp => tp.AssemblyLines)
        .HasForeignKey(al => al.TaskProductId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AssemblyLine>()
        .HasOne(al => al.User)
        .WithMany(u => u.AssemblyLines)  
        .HasForeignKey(al => al.UserId)
        .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
