using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AsyncWarehouse.Infrastructure;

/// <summary>
/// Defines a contract for sending messages to a message broker.
/// </summary>
public interface IMessageProducer
{
    /// <summary>
    /// Sends a message to the message broker asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the message being sent.</typeparam>
    /// <param name="message">The message object to send.</param>
    /// <param name="routingKey">The routing key used by the broker to direct the message to the appropriate queue.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    Task SendMessage<T>(T message, string routingKey);
}

/// <summary>
/// A RabbitMQ-based implementation of <see cref="IMessageProducer"/>.
/// </summary>
public class RabbitMqProducer : IMessageProducer
{
    /// <inheritdoc />
    public async Task SendMessage<T>(T message, string routingKey)
    {
        var factory = new ConnectionFactory 
        { 
            HostName = "localhost", 
            Port = 5670
        };
        using var connect = await factory.CreateConnectionAsync();
        using var channel = await connect.CreateChannelAsync();

        await channel.ExchangeDeclareAsync("delivery_exchange", ExchangeType.Topic, durable: true);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

       await channel.BasicPublishAsync(exchange: "delivery_exchange", routingKey: routingKey, body: body);
    }
}