using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Entities.POCOs;
using OnlineStoreApp.Repository.EFCore.EntitiesConfiguration;

namespace OnlineStoreApp.Repository.EFCore.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaims> UserClaims { get; set; }
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new FoodConfiguration());

            //base.OnModelCreating(modelBuilder);
        }
    }
}
