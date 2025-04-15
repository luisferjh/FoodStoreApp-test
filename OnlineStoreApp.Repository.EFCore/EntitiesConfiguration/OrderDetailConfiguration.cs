using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Repository.EFCore.EntitiesConfiguration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {

        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(od => od.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int");

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .IsRequired();

            //builder.Property(x => x.OrderId)
            //    .HasColumnName("order_id")
            //    .HasColumnType("int")
            //    .IsRequired();

            builder.Property(x => x.FoodId)
                .HasColumnName("food_id")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(od => od.Quantity)
                .HasColumnName("quantity")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(od => od.SubTotal)
                .HasColumnName("subtotal")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(od => od.Order)
              .WithMany(o => o.OrderDetails)
              .HasForeignKey(od => od.OrderId)
              .IsRequired();

            builder.HasOne(od => od.Food)
               .WithMany()
               .HasForeignKey(od => od.FoodId)
               .IsRequired();
        }
    }
}
