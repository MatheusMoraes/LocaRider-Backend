using LocaRider.Application.DTO.Driver;
using LocaRider.Domain.Entities.Driver;

namespace LocaRider.Application.Interfaces.Drivers
{
    public interface IDriversService
    {
        Task<string> UpdateDriverCnhImageAsync(string driverId, DriverBase64ImageDTO driverBase64ImageDTO);
        Task<DriverDTO?> CreateDriverAsync(DriverDTO driverDTO);
    }
}
