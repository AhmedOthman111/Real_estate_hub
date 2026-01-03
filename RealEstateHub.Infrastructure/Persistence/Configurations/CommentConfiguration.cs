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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Message)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(x => x.Ad)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.AdId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<AppUser>() 
                .WithMany()
                .HasForeignKey(x => x.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
