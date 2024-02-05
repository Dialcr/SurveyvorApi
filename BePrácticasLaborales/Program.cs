
using BePrácticasLaborales;
using BePrácticasLaborales.DataAcces;
using Microsoft.EntityFrameworkCore;

//using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

var conectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var loggingLevel = builder.Configuration.GetSection("Logging");


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();   
builder.Services.AddEndpointsApiExplorer();

//todo: add logging at database?
builder.Services.AddDbContext<EntityDbContext>(options => 
    options.UseNpgsql(conectionString)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging());
builder.Services.SetAuthentication(builder.Configuration);
builder.Services.SetServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers()
    .RequireAuthorization();

app.Run();

