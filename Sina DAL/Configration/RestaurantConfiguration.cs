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
    public class RestaurantConfiguration
    : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(r => r.Location)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(r => r.AverageRate)
                   .HasPrecision(2, 1);

            builder.Property(r => r.IsActive)
                   .HasDefaultValue(false);

            builder.HasOne(r => r.Owner)
                   .WithMany(u => u.Restaurants)
                   .HasForeignKey(r => r.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
