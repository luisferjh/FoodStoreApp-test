using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Repository.EFCore.EntitiesConfiguration
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.ToTable("Food");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int");

            builder.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(200)")
                .HasColumnName("description");

            builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("price");

            builder.Property(x => x.QuantityAvailable)
               .HasColumnName("quantity")
               .HasColumnType("int");

            builder.HasOne(f => f.Category)
                .WithMany(c => c.Foods)
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
