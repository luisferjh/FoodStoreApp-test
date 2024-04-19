using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Repository.EFCore.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
             .HasColumnType("int")
             .HasColumnName("id");

            builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasColumnType("nvarchar(50)")
            .IsRequired();

            builder.Property(s => s.Email)
            .HasColumnType("nvarchar(50)")
             .HasColumnName("email")
             .IsRequired();

            builder.Property(s => s.Password)
            .HasColumnType("nvarchar(max)")
            .HasColumnName("password")
            .IsRequired();

            builder.Property(s => s.State)
            .HasColumnType("int")
            .HasColumnName("id_state")
            .IsRequired();

            builder.Property(s => s.CreatedDate)
            .HasColumnType("date")
            .HasColumnName("created_date")
            .IsRequired();

            builder.HasQueryFilter(x => x.State == 1);
        }
    }
}
