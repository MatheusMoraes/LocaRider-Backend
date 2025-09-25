
using LocaRider.Domain.Entities.Motorcycles;

namespace LocaRider.Domain.Interfaces.Motorcycles
{
    public interface IMotorcyclesRepository
    {
        Task<Motorcycle> AddAsync(Motorcycle motorcycle);
        Task DeleteAsync(Motorcycle existingMotorcycle);
        Task<IEnumerable<Motorcycle>> GetAllAsync();
        Task<Motorcycle?> GetMotorcycleByPlateAsync(string plate);
        Task<Motorcycle?> GetMotorcycleByIdAsync(string id);
        Task<bool> VerifyExistsMotorcycleByPlateOrIdAsync(string plate, string id);
        Task UpdateAsync(Motorcycle existingMotorcycle);
        Task<bool> ExistsByPlateAsync(string plate, string excludeId);
    }
}
