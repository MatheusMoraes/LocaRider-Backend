using AutoMapper;
using LocaRider.Application.DTO.Motorcycle;
using LocaRider.Application.Interfaces.Motorcycle;
using LocaRider.Domain.Entities.Motorcycles;
using LocaRider.Domain.Interfaces.Motorcycles;
using System.Collections.Generic;

namespace LocaRider.Application.Services.Motorcycles
{
    public class MotorcyclesService : IMotorcyclesService
    {
        private readonly IMotorcyclesRepository _motorcyclesRepository;
        private readonly IMapper _mapper;

        public MotorcyclesService(IMotorcyclesRepository motorcyclesRepository, IMapper mapper)
        {
            _motorcyclesRepository = motorcyclesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MotorcycleDTO>> GetAllMotorcyclesAsync(string plate)
        {
            
            if (string.IsNullOrEmpty(plate))
            {
                var motorcycles = await _motorcyclesRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<MotorcycleDTO>>(motorcycles);
            }
            else
            {
                return _mapper.Map<IEnumerable<MotorcycleDTO>>(await _motorcyclesRepository.GetMotorcycleByPlateAsync(plate));
            }

        }

        public async Task<MotorcycleDTO?> GetMotorcycleByIdAsync(string id)
        {
            var motorcycle = await _motorcyclesRepository.GetMotorcycleByIdAsync(id);
            return motorcycle == null ? null : _mapper.Map<MotorcycleDTO>(motorcycle);
        }

        public async Task<MotorcycleDTO?> CreateMotorcycleAsync(MotorcycleDTO motorcycleDTO)
        {
            if (await _motorcyclesRepository.VerifyExistsMotorcycleByPlateOrIdAsync(motorcycleDTO.placa, motorcycleDTO.identificador))
                return null;

            var entity = _mapper.Map<Motorcycle>(motorcycleDTO);
            var created = await _motorcyclesRepository.AddAsync(entity);

            return motorcycleDTO;
        }

        public async Task<bool> UpdateMotorcycleAsync(string id, MotorcyclePlateDTO motorcyclePlateDTO)
        {
            var existingMotorcycle = await _motorcyclesRepository.GetMotorcycleByIdAsync(id);
            if (existingMotorcycle is null) return false;

            var plateExists = await _motorcyclesRepository.ExistsByPlateAsync(motorcyclePlateDTO.placa, id);
            if (plateExists) return false;

            existingMotorcycle.Plate = motorcyclePlateDTO.placa;
            await _motorcyclesRepository.UpdateAsync(existingMotorcycle);
            return true;
        }

        public async Task<bool> DeleteMotorcycleAsync(string id)
        {
            var existingMotorcycle = await _motorcyclesRepository.GetMotorcycleByIdAsync(id);
            if (existingMotorcycle == null) return false;

            await _motorcyclesRepository.DeleteAsync(existingMotorcycle);
            return true;
        }
    }
}
