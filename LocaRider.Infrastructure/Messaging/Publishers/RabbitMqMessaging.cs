using LocaRider.Domain.Events;
using LocaRider.Infrastructure.Messaging.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMqPublisher : IEventPublisher
{
    private readonly string _hostname = "localhost";
    private readonly string _queueName = "motorcycle_created";

    public async Task PublishMotorcycleCreatedAsync(MotorcycleNotification motorcycleEvent)
    {
        var factory = new ConnectionFactory { HostName = _hostname };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
             queue: _queueName,
             durable: true,
             exclusive: false,
             autoDelete: false,
             arguments: null
         );

        var message = JsonSerializer.Serialize(motorcycleEvent);
        var body = Encoding.UTF8.GetBytes(message);

       
        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: _queueName,
            mandatory: false,
            body: body
        );
    }
}
