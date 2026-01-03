using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RealEstateHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateHub.Infrastructure.Data.Configurations
{
    public class AdPhotoConfiguration : IEntityTypeConfiguration<AdPhoto>
    {
        public void Configure(EntityTypeBuilder<AdPhoto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageUrl)
                .IsRequired();

            builder.HasOne(x => x.Ad)
                  .WithMany(a => a.Photos)
                  .HasForeignKey(x => x.AdId)
                  .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
