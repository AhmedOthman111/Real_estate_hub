using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealEstateHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Stars)
                   .IsRequired();

            builder.Property(x => x.Review)
                   .HasMaxLength(500);

            builder.HasIndex(x => new { x.OwnerId, x.CustomerID })
                   .IsUnique();

            builder.HasOne(x => x.Owner)
                   .WithMany()
                   .HasForeignKey(x => x.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>()
                   .WithMany()
                   .HasForeignKey(x => x.CustomerID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CustomerID)
                   .IsRequired();
        }
    }

}
