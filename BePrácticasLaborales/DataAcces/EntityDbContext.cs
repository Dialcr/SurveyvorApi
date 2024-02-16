using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BePrácticasLaborales.DataAcces;

public class EntityDbContext  : IdentityDbContext<User,IdentityRole<int>, int > //IdentityDbContext 
{
    public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> User { get; set; }
//public DbSet<IdentityRole> IdentityRole { get; set; }
    public DbSet<Organization> Organization { get; set; }
/*
*/
    public DbSet<University> University { get; set; }
    public DbSet<Ministery> Ministery { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole<int>>()
            .HasData(new IdentityRole<int>[]
            {
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = RoleNames.Admin,
                    NormalizedName = RoleNames.Admin.Trim().ToUpper().Replace(" ", ""),
                },
                new IdentityRole<int>
                {
                    Id = 2,
                    Name = RoleNames.Organization,
                    NormalizedName = RoleNames.Organization.Trim().ToUpper().Replace(" ", ""),
                }
            });

        modelBuilder.Entity<User>()
            .HasOne(x => x.Organization)
            .WithMany(x=>x.Users);
        
        
        modelBuilder.Entity<Ministery>()
            .HasMany(x => x.Universities)
            .WithOne(x => x.Ministery);
            

    }
}