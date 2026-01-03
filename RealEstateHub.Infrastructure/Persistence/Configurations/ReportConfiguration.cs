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
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Reason)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.HasOne(x => x.Ad)
                   .WithMany()
                   .HasForeignKey(x => x.AdId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>() 
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
