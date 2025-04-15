using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Repository.EFCore.EntitiesConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");


            builder.HasKey(o => o.Id);

            //builder.Property(x => x.Id)
            //   .HasColumnName("id")
            //   .HasColumnType("int");

            builder.Property(x => x.Id)
               .HasColumnName("id")
               .HasColumnType("uniqueidentifier")
               .ValueGeneratedNever();

            builder.Property(x => x.UserId)
              .HasColumnName("user_id")
              .HasColumnType("int")
              .IsRequired();

            //builder.HasOne(o => o.User)
            //    .WithMany(u => u.Orders) 
            //    .HasForeignKey(o => o.UserId) 
            //    .IsRequired();           

            //builder.HasOne<IdentityUser>()
            //   .WithMany() // Opcional: especifica WithMany si la relación es uno a muchos
            //   .HasForeignKey("UserId");

            builder.Property(o => o.Date)
                .HasColumnName("date")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(o => o.Total)
                .HasColumnName("total")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.Status)
                .HasColumnName("status")
                .HasColumnType("nvarchar(30)")
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(o => o.OrderDetails)
               .WithOne(od => od.Order)
               .HasForeignKey(od => od.OrderId);
        }
    }
}
