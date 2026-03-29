using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WarehouseMS.Application.DTOs.Messages;

namespace WarehouseMS.Infrastructure.BackgroundJobs;

/// <summary>
/// A background worker that processes drone delivery tasks from RabbitMQ.
/// </summary>
public class DroneDeliveryWorker : BackgroundService
{
    private readonly ILogger<DroneDeliveryWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    
    // В версии 7.x используется IChannel вместо старого IModel
    private IConnection? _connection;
    private IChannel? _channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="DroneDeliveryWorker"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="scopeFactory">The factory for creating service scopes.</param>
    public DroneDeliveryWorker(ILogger<DroneDeliveryWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Executes the background task of listening for and processing drone delivery messages.
    /// </summary>
    /// <param name="stoppingToken">The cancellation token to observe.</param>
    /// <returns>A task that represents the long-running execution.</returns>
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Инициализация асинхронного RabbitMQ Consumer'а...");

        var factory = new ConnectionFactory 
        { 
            HostName = "localhost", 
            Port = 5670
        };
        
        // 1. Асинхронно создаем подключение и канал
        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        // 2. Асинхронно объявляем инфраструктуру (Exchange, Queue, Bind)
        await _channel.ExchangeDeclareAsync(exchange: "delivery_exchange", type: ExchangeType.Topic, durable: true,
            autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(queue: "drone_queue", durable: true, exclusive: false, autoDelete: false,
            cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queue: "drone_queue", exchange: "delivery_exchange", routingKey: "drone",
            cancellationToken: stoppingToken);
        
        // 3. Создаем асинхронного слушателя
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        // Подписываемся на событие ReceivedAsync
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var dispatchData = JsonSerializer.Deserialize<DispatchPalletMessage>(message);

            _logger.LogInformation($"[Дрон] Асинхронно получена задача на палетку: {dispatchData?.PalletId}");

            // Создаем scope для работы с БД или scoped-сервисами
            using (var scope = _scopeFactory.CreateScope())
            {
                 var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                 
                 var pallet = await context.Pallets.FirstOrDefaultAsync(
                     p => p.Id == dispatchData!.PalletId, 
                     cancellationToken: stoppingToken);

                 if (pallet == null)
                 {
                     _logger.LogWarning($"[Дрон] Палетка {dispatchData!.PalletId} не найдена в БД.");
                     return; 
                 }

                 context.Pallets.Remove(pallet);
                 await context.SaveChangesAsync(stoppingToken);
                 
                _logger.LogInformation($"[Дрон] Успешное удаление палетки {dispatchData!.PalletId} из БД");
            }

            _logger.LogInformation($"[Дрон] Палетка {dispatchData?.PalletId} обработана.");

            // 4. Асинхронно подтверждаем успешную обработку
            await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false,
                cancellationToken: stoppingToken);
        };
        
        // 5. Запускаем консьюмера
        await _channel.BasicConsumeAsync(queue: "drone_queue", autoAck: false, consumer: consumer,
            cancellationToken: stoppingToken);
        
        // 6. Удерживаем ExecuteAsync в рабочем состоянии до получения сигнала отмена (остановки приложения)
        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException)
        {
            // Нормальное поведение при остановке приложения (срабатывает stoppingToken)
        }
    }
    
    /// <summary>
    /// Safely terminates the RabbitMQ consumer and closes all connections.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task that represents the asynchronous stop operation.</returns>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Остановка RabbitMQ Consumer'а и закрытие соединений...");

        if (_channel is not null) 
        {
            await _channel.CloseAsync(cancellationToken);
            await _channel.DisposeAsync();
        }

        if (_connection is not null) 
        {
            await _connection.CloseAsync(cancellationToken);
            await _connection.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}