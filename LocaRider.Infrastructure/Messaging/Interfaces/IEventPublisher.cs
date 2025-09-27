using LocaRider.Domain.Events;

namespace LocaRider.Infrastructure.Messaging.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishMotorcycleCreatedAsync(MotorcycleNotification motorcycleEvent);
    }
}
