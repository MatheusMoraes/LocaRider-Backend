using AutoMapper;
using LocaRider.Application.DTO.Motorcycle;
using LocaRider.Application.Interfaces.Motorcycle;
using LocaRider.Domain.Entities.Motorcycles;
using LocaRider.Domain.Interfaces.Motorcycles;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LocaRider.Application.Services.Motorcycles
{
    public class MotorcyclesService : IMotorcyclesService
    {
        private readonly IMotorcyclesRepository _motorcyclesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MotorcyclesService> _logger;

        public MotorcyclesService(IMotorcyclesRepository motorcyclesRepository, IMapper mapper, ILogger<MotorcyclesService> logger)
        {
            _motorcyclesRepository = motorcyclesRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MotorcycleDTO>> GetAllMotorcyclesAsync(string plate)
        {
            _logger.LogInformation("Buscando motos. Filtro de placa: {Plate}", plate);

            try
            {
                if (string.IsNullOrEmpty(plate))
                {
                    var motorcycles = await _motorcyclesRepository.GetAllAsync();
                    _logger.LogInformation("Total de motos encontradas: {Count}", motorcycles.Count());
                    return _mapper.Map<IEnumerable<MotorcycleDTO>>(motorcycles);
                }
                else
                {
                    var motorcycles = await _motorcyclesRepository.GetMotorcycleByPlateAsync(plate);
                    _logger.LogInformation("Motos encontrada: {motorcyless}",  motorcycles);
                    return _mapper.Map<IEnumerable<MotorcycleDTO>>(motorcycles);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar motos com filtro de placa: {Plate}", plate);
                throw;
            }
        }

        public async Task<MotorcycleDTO?> GetMotorcycleByIdAsync(string id)
        {
            _logger.LogInformation("Buscando moto pelo ID: {MotorcycleId}", id);

            try
            {
                var motorcycle = await _motorcyclesRepository.GetMotorcycleByIdAsync(id);
                if (motorcycle == null)
                {
                    _logger.LogWarning("Moto não encontrada para o ID: {MotorcycleId}", id);
                    return null;
                }

                _logger.LogInformation("Moto encontrada: {MotorcycleId}", id);
                return _mapper.Map<MotorcycleDTO>(motorcycle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar moto pelo ID: {MotorcycleId}", id);
                throw;
            }
        }

        public async Task<MotorcycleDTO?> CreateMotorcycleAsync(MotorcycleDTO motorcycleDTO)
        {
            _logger.LogInformation("Iniciando criação de moto. Placa: {Plate}, ID: {Id}", motorcycleDTO.placa, motorcycleDTO.identificador);

            try
            {
                if (await _motorcyclesRepository.VerifyExistsMotorcycleByPlateOrIdAsync(motorcycleDTO.placa, motorcycleDTO.identificador))
                {
                    _logger.LogWarning("Moto já existe. Placa: {Plate}, ID: {Id}", motorcycleDTO.placa, motorcycleDTO.identificador);
                    return null;
                }

                var entity = _mapper.Map<Motorcycle>(motorcycleDTO);
                var created = await _motorcyclesRepository.AddAsync(entity);

                _logger.LogInformation("Moto criada com sucesso. Placa: {Plate}, ID: {Id}", created.Plate, created.MotorcycleId);
                return _mapper.Map<MotorcycleDTO>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar moto. Placa: {Plate}, ID: {Id}", motorcycleDTO.placa, motorcycleDTO.identificador);
                throw;
            }
        }

        public async Task<bool> UpdateMotorcycleAsync(string id, MotorcyclePlateDTO motorcyclePlateDTO)
        {
            _logger.LogInformation("Atualizando placa da moto. ID: {MotorcycleId}, Nova Placa: {Plate}", id, motorcyclePlateDTO.placa);

            try
            {
                var existingMotorcycle = await _motorcyclesRepository.GetMotorcycleByIdAsync(id);
                if (existingMotorcycle is null)
                {
                    _logger.LogWarning("Moto não encontrada para atualização. ID: {MotorcycleId}", id);
                    return false;
                }

                var plateExists = await _motorcyclesRepository.ExistsByPlateAsync(motorcyclePlateDTO.placa, id);
                if (plateExists)
                {
                    _logger.LogWarning("A placa já está cadastrada para outra moto. Placa: {Plate}", motorcyclePlateDTO.placa);
                    return false;
                }

                existingMotorcycle.Plate = motorcyclePlateDTO.placa;
                await _motorcyclesRepository.UpdateAsync(existingMotorcycle);

                _logger.LogInformation("Placa da moto atualizada com sucesso. ID: {MotorcycleId}, Nova Placa: {Plate}", id, motorcyclePlateDTO.placa);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar moto. ID: {MotorcycleId}, Nova Placa: {Plate}", id, motorcyclePlateDTO.placa);
                throw;
            }
        }

        public async Task<bool> DeleteMotorcycleAsync(string id)
        {
            _logger.LogInformation("Removendo moto. ID: {MotorcycleId}", id);

            try
            {
                var existingMotorcycle = await _motorcyclesRepository.GetMotorcycleByIdAsync(id);
                if (existingMotorcycle == null)
                {
                    _logger.LogWarning("Moto não encontrada para remoção. ID: {MotorcycleId}", id);
                    return false;
                }

                await _motorcyclesRepository.DeleteAsync(existingMotorcycle);
                _logger.LogInformation("Moto removida com sucesso. ID: {MotorcycleId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover moto. ID: {MotorcycleId}", id);
                throw;
            }
        }
    }
}
