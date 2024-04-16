﻿using DataAcces.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BePrácticasLaborales.DataAcces;

public class EntityDbContext : IdentityDbContext<User, IdentityRole<int>, int> //IdentityDbContext
{
    public EntityDbContext(DbContextOptions<EntityDbContext> options)
        : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<IdentityRole> IdentityRole { get; set; }

    public DbSet<University> University { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyAsk> SurveyAsks { get; set; }
    public DbSet<SurveyResponse> SurveyResponses { get; set; }
    public DbSet<ResponsePosibility> ResponsePosibilities { get; set; }
    public DbSet<Application> Applications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<IdentityRole<int>>()
            .HasData(
                new IdentityRole<int>[]
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
                }
            );

        modelBuilder.Entity<User>().HasOne(x => x.Organization).WithMany(x => x.Users);

        modelBuilder.Entity<Survey>().HasOne(x => x.Organization).WithMany(x => x.Surveys);
        modelBuilder.Entity<Survey>().HasMany(x => x.SurveyAsks).WithOne(x => x.Survey);
        modelBuilder
            .Entity<SurveyAsk>()
            .HasMany(x => x.ResponsePosibilities)
            .WithOne(x => x.SurveyAsk);

        modelBuilder
            .Entity<SurveyResponse>()
            .HasOne(sr => sr.ResponsePosibility)
            .WithMany(rp => rp.SurveyResponses);

        modelBuilder.Entity<SurveyAsk>().HasIndex(x => x.SurveyId);

        modelBuilder.Entity<ResponsePosibility>().HasIndex(x => x.SuveryAskId);

        modelBuilder.Entity<SurveyAsk>().HasIndex(x => x.Description);

        modelBuilder.Entity<ResponsePosibility>().HasIndex(x => x.ResponseValue);

        modelBuilder.Entity<University>().HasIndex(x => x.Name).IsUnique();
    }
}
