using LocaRider.Domain.Events;
using LocaRider.Infrastructure.Data.Context;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace LocaRider.Infrastructure.Messaging.Consumers
{
    public class CreatedMotorcycleConsumer
    {
        private readonly ConnectionFactory _factory;
        private readonly LocaRiderMongoDbContext _mongoContext;
        private const string QueueName = "moto_cadastrada";

        public CreatedMotorcycleConsumer(LocaRiderMongoDbContext mongoContext)
        {
            _mongoContext = mongoContext;
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await using var connection = await _factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken
            );

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var evento = JsonSerializer.Deserialize<MotorcycleNotification>(message);

                    if (evento != null && evento.Year == 2024)
                    {
                        var notification = new MotorcycleNotification
                        {
                            MotorcycleId = evento.MotorcycleId,
                            Model = evento.Model,
                            Year = evento.Year,
                            ReceivedAt = DateTime.UtcNow
                        };

                        await _mongoContext.Notifications.InsertOneAsync(notification);
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro no Consumer: {ex.Message}");
                    await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken
            );

            Console.WriteLine("✅ Consumer iniciado. Aguardando mensagens...");
            await Task.Delay(-1, cancellationToken);
        }
    }
}
