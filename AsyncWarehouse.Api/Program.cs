using System.Reflection;
using Microsoft.EntityFrameworkCore;
using AsyncWarehouse.Application;
using AsyncWarehouse.Application.AutoMapper;
using AsyncWarehouse.Application.Interfaces;
using AsyncWarehouse.Infrastructure;
using AsyncWarehouse.Infrastructure.BackgroundJobs;
using AsyncWarehouse.Infrastructure.Repositories;
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
builder.Services.AddScoped<IPalletRepository, PalletRepository>();

builder.Services.AddSingleton<IMessageProducer, RabbitMqProducer>();
builder.Services.AddHostedService<TruckDeliveryWorker>();
builder.Services.AddHostedService<ShipDeliveryWorker>();
builder.Services.AddHostedService<DroneDeliveryWorker>();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();