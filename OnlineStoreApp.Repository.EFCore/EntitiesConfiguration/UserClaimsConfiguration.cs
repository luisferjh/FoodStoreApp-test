using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStoreApp.Entities.POCOs;

namespace OnlineStoreApp.Repository.EFCore.EntitiesConfiguration
{
    public class UserClaimsConfiguration : IEntityTypeConfiguration<UserClaims>
    {
        public void Configure(EntityTypeBuilder<UserClaims> builder)
        {
            builder.ToTable("UserClaims");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("int");

            builder.Property(x => x.UserId)
               .HasColumnName("user_id")
               .HasColumnType("int")
               .IsRequired();

            builder.Property(x => x.ClaimType)
                .HasColumnName("claim_type")
                .HasColumnType("nvarchar(30)")
                .IsRequired();

            builder.Property(x => x.ClaimValue)
              .HasColumnName("claim_value")
              .HasColumnType("nvarchar(20)")
              .IsRequired();

            builder.HasOne(s => s.User)
               .WithMany(g => g.UserClaims)
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
