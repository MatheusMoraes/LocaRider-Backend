using LocaRider.Domain.Entities.Rental;

namespace LocaRider.Domain.Interfaces.Rentals
{
    public interface IRentalsRepository
    {
        Task<Rental?> GetByIdAsync(string rentalId);
        Task AddAsync(Rental rental);
        Task UpdateAsync(Rental rental);
        Task<Rental?> GetActiveRentalByMotorcycleAsync(string motorcycleId);
    }
}
