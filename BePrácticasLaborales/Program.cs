
using BePrácticasLaborales.DataAcces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

//using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

var conectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddDbContext<EntityDbContext>(options => options.UseNpgsql(conectionString));

builder.Services.AddIdentityCore<User>(options =>
    {
        options.Password.RequiredLength =
            builder.Configuration.GetSection("Authentication").GetValue<int>("RequiredLength");
        options.Password.RequireNonAlphanumeric = builder.Configuration.GetSection("Authentication")
            .GetValue<bool>("RequireNonAlphanumeric");
        options.Password.RequireDigit =
            builder.Configuration.GetSection("Authentication").GetValue<bool>("RequireDigit");
        options.Password.RequireLowercase =
            builder.Configuration.GetSection("Authentication").GetValue<bool>("RequireLowercase");
        options.Password.RequireUppercase =
            builder.Configuration.GetSection("Authentication").GetValue<bool>("RequireUppercase");
        
        options.SignIn.RequireConfirmedEmail = true;
            
    })
    .AddEntityFrameworkStores<EntityDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();
    
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

