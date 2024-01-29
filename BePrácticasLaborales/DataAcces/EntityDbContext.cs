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
     
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    

    base.OnModelCreating(modelBuilder);

     modelBuilder.Entity<IdentityRole<int>>()
         .HasData(new IdentityRole<int>[]
         {
             new IdentityRole<int>
             {
                 Id = 1,
                 Name = RoleNames.Support,
                 NormalizedName = RoleNames.Support.Trim().ToLower().Replace(" ", ""),
             },
             new IdentityRole<int>
             {
                 Id = 2,
                 Name = RoleNames.Customer,
                 NormalizedName = RoleNames.Customer.Trim().ToLower().Replace(" ", ""),
             }
         });
        
                    
}
}
