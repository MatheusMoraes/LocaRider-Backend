using LocaRider.Domain.Entities.Driver;

namespace LocaRider.Domain.Interfaces.Drivers
{
    public interface IDriversRepository
    {
        Task<Driver> AddAsync(Driver driver);
        Task<bool> ExistsByCNPJAsync(string cnpj);
        Task<bool> AddBase64ImageAsync(string id, string base64Image);
        Task<bool> ExistsByCNHNumberAsync(string cnhNumber);
        Task<Driver?> GetByIdAsync(string id);
        Task<Driver> UpdateAsync(Driver driver);
    }
}
