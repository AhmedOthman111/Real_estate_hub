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
    public class ReplyConfiguration : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Message)
                   .IsRequired()
                   .HasMaxLength(400);

            builder.HasOne(r => r.Comment)
                   .WithOne(c => c.Reply)
                   .HasForeignKey<Reply>(r => r.CommentId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(x => x.Owner)
                   .WithMany()
                   .HasForeignKey(x => x.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
