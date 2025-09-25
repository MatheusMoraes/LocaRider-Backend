
using LocaRider.Application.DTO.Rental;
using LocaRider.Domain.Entities.Rental;

namespace LocaRider.Application.Interfaces.Rentals
{
    public interface IRentalsService
    {
        Task<RentalDTO> CreateRentalAsync(RentalDTO rentalDTO);
        Task<RentalDTO> GetRentalByIdAsync(string rentalId);
        Task<bool> SetReturnDateAsync(string rentalId, RentalDevolutionDTO rentalDevolutionDTO);
    }
}
