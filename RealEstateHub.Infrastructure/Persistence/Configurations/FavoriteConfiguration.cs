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
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.CustomerId, x.AdId })
                   .IsUnique();

            builder.HasOne(x => x.Ad)
                   .WithMany()
                   .HasForeignKey(x => x.AdId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>() // customer
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }


}
