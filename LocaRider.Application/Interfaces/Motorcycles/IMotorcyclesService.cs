using LocaRider.Application.DTO.Motorcycle;

namespace LocaRider.Application.Interfaces.Motorcycle
{
    public interface IMotorcyclesService
    {
        Task<MotorcycleDTO?> CreateMotorcycleAsync(MotorcycleDTO motorcycleDTO);
        Task<bool> DeleteMotorcycleAsync(string id);
        Task<IEnumerable<MotorcycleDTO>> GetAllMotorcyclesAsync(string plate);
        Task<MotorcycleDTO?> GetMotorcycleByIdAsync(string id);
        Task<bool> UpdateMotorcycleAsync(string id, MotorcyclePlateDTO motorcyclePlateDTO);
    }
}
