using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sina_DAL.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

public class SinaDbContext: IdentityDbContext<ApplicationUser>
{
    public SinaDbContext(DbContextOptions<SinaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(SinaDbContext).Assembly
        );
    }

}
