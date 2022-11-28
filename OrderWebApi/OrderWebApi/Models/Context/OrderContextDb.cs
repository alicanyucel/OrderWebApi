using Microsoft.EntityFrameworkCore;
using OrderWebApi.Models.Entities;

namespace OrderWebApi.Models.Context
{
    public class OrderContextDb:DbContext
    {
        public OrderContextDb(DbContextOptions options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail>  OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasOne(d => d.Product).WithMany(p => p.OrderDetails).HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<OrderDetail>().HasOne(p=>p.Order).WithMany(p => p.OrderDetails).HasForeignKey(p => p.OrderId);

        }
    }
}
