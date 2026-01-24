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
    public class ItemConfiguration
    : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(i => i.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(i => i.Price)
                   .HasPrecision(10, 2);

            builder.Property(i => i.IsAvailable)
                   .HasDefaultValue(true);

            builder.HasOne(i => i.Category)
                   .WithMany(c => c.Items)
                   .HasForeignKey(i => i.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
