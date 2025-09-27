using LocaRider.API.Utils.Motorcycles;
using LocaRider.Application.AutoMapper;
using LocaRider.Application.Interfaces.Drivers;
using LocaRider.Application.Interfaces.Motorcycle;
using LocaRider.Application.Interfaces.Rentals;
using LocaRider.Application.Interfaces.Storages;
using LocaRider.Application.Services.Drivers;
using LocaRider.Application.Services.LocalStorages;
using LocaRider.Application.Services.Motorcycles;
using LocaRider.Application.Services.Rentals;
using LocaRider.Domain.Interfaces.Drivers;
using LocaRider.Domain.Interfaces.Motorcycles;
using LocaRider.Domain.Interfaces.RentalPlans;
using LocaRider.Domain.Interfaces.Rentals;
using LocaRider.Infrastructure.Data.Context;
using LocaRider.Infrastructure.Data.Extensions;
using LocaRider.Infrastructure.Data.Repositories.Drivers;
using LocaRider.Infrastructure.Data.Repositories.Motorcycles;
using LocaRider.Infrastructure.Data.Repositories.RentalPlans;
using LocaRider.Infrastructure.Data.Repositories.Rentals;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddLocaRiderData(builder.Configuration);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new MotorcycleMappingProfile());
    cfg.AddProfile(new DriverMappingProfile());
    cfg.AddProfile(new RentalMappingProfile());
});


builder.Services.AddScoped<IMotorcyclesRepository, MotorcyclesRepository>();
builder.Services.AddScoped<IMotorcyclesService, MotorcyclesService>();

builder.Services.AddScoped<IDriversService, DriversService>();
builder.Services.AddScoped<IDriversRepository, DriversRepository>();

builder.Services.AddScoped<IStoragesService, LocalStorageService>();

builder.Services.AddScoped<IRentalPlansRepository, RentalPlansRepository>();

builder.Services.AddScoped<IRentalsService, RentalsService>();
builder.Services.AddScoped<IRentalsRepository, RentalsRepository>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ExampleFilters(); 
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<MotorcyclesObjectExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<MotorcyclesPlateExample>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // HTTP
    options.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LocaRiderDbContext>();
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
