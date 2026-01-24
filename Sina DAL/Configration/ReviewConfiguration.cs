using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sina_DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_DAL.Configration
{
    public class ReviewConfiguration
    : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.Rate)
                   .IsRequired();

            builder.Property(r => r.Comment)
                   .HasMaxLength(500);

            builder.Property(r => r.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(r => new { r.UserId, r.RestaurantId })
                   .IsUnique();

            builder.HasCheckConstraint(
                "CK_Review_Rate",
                "Rate BETWEEN 1 AND 5"
            );

            builder.HasOne(r => r.User)
                   .WithMany(u => u.Reviews)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Restaurant)
                   .WithMany(r => r.Reviews)
                   .HasForeignKey(r => r.RestaurantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
