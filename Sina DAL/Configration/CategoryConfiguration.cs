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
    public class CategoryConfiguration
    : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasOne(c => c.Menu)
                   .WithMany(m => m.Categories)
                   .HasForeignKey(c => c.MenuId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
