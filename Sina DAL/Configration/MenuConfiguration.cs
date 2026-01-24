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
    public class MenuConfiguration
    : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasOne(m => m.Restaurant)
                   .WithMany(r => r.Menus)
                   .HasForeignKey(m => m.RestaurantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
