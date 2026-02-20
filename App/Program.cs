using Microsoft.EntityFrameworkCore;
using AsyncWarehouse.Application;
using AsyncWarehouse.Application.AutoMapper;
using AsyncWarehouse.Infrastructure;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Выведет в консоль текущую среду
Console.WriteLine($"Current Environment: {builder.Environment.EnvironmentName}");

// Add support of Controllers
builder.Services.AddControllers();

var mapperConfig = new MapperConfiguration(config => 
    {
        config.AddProfile(new MappingProfile());     // Профиль из Application
    },
    LoggerFactory.Create(b => b.AddConsole()));
IMapper? mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddKeyedScoped<IDeliveryService, TruckDelivery>("truck");
builder.Services.AddKeyedScoped<IDeliveryService, ShipDelivery>("ship");
builder.Services.AddKeyedScoped<IDeliveryService, DroneDelivery>("drone");

// Add Swagger and documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to Postgresql
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<WarehouseService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();