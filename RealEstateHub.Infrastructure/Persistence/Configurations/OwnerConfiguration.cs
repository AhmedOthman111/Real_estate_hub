using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Infrastructure.Data.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AppUserId)
                .IsRequired();

            builder.Property(x => x.WhatsappNumber)
                .IsRequired().HasMaxLength(11);
         
            builder.Property(x => x.Bio)
                .HasMaxLength(500);

            builder.Property(x => x.CompanyName)
                   .HasMaxLength(200);

            builder.Property(x => x.AverageRating)
                   .HasDefaultValue(0);

            builder.HasOne<AppUser>()   
                   .WithOne()
                   .HasForeignKey<Owner>(x => x.AppUserId)
                   .OnDelete(DeleteBehavior.Restrict);



        }

    }
}
