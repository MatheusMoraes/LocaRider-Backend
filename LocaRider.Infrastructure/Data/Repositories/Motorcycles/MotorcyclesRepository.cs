using LocaRider.Domain.Entities.Motorcycles;
using LocaRider.Domain.Interfaces.Motorcycles;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LocaRider.Infrastructure.Data.Repositories.Motorcycles
{
    public class MotorcyclesRepository : IMotorcyclesRepository
    {
        private readonly LocaRiderDbContext _db;
        private readonly ILogger<MotorcyclesRepository> _logger;

        public MotorcyclesRepository(LocaRiderDbContext db, ILogger<MotorcyclesRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Motorcycle> AddAsync(Motorcycle motorcycle)
        {
            _logger.LogInformation("Adicionando nova moto com ID {MotorcycleId} e placa {Plate}", motorcycle.MotorcycleId, motorcycle.Plate);

            try
            {
                _db.Motorcycles.Add(motorcycle);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Moto {MotorcycleId} adicionada com sucesso", motorcycle.MotorcycleId);
                return motorcycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar moto {MotorcycleId} com placa {Plate}", motorcycle.MotorcycleId, motorcycle.Plate);
                throw;
            }
        }

        public async Task DeleteAsync(Motorcycle motorcycle)
        {
            _logger.LogInformation("Removendo moto {MotorcycleId} com placa {Plate}", motorcycle.MotorcycleId, motorcycle.Plate);

            try
            {
                _db.Motorcycles.Remove(motorcycle);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Moto {MotorcycleId} removida com sucesso", motorcycle.MotorcycleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover moto {MotorcycleId} com placa {Plate}", motorcycle.MotorcycleId, motorcycle.Plate);
                throw;
            }
        }

        public async Task<IEnumerable<Motorcycle>> GetAllAsync()
        {
            _logger.LogInformation("Consultando todas as motos");

            try
            {
                var motorcycles = await _db.Motorcycles.AsNoTracking().ToListAsync();
                _logger.LogInformation("{Count} motos encontradas", motorcycles.Count);
                return motorcycles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar todas as motos");
                throw;
            }
        }

        public async Task<Motorcycle?> GetMotorcycleByIdAsync(string id)
        {
            _logger.LogInformation("Buscando moto pelo ID {MotorcycleId}", id);

            try
            {
                var motorcycle = await _db.Motorcycles.FirstOrDefaultAsync(m => m.MotorcycleId == id);
                if (motorcycle is null)
                    _logger.LogWarning("Moto não encontrada. ID {MotorcycleId}", id);
                else
                    _logger.LogInformation("Moto encontrada. ID {MotorcycleId}", id);

                return motorcycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar moto pelo ID {MotorcycleId}", id);
                throw;
            }
        }

        public async Task<bool> VerifyExistsMotorcycleByPlateOrIdAsync(string plate, string id)
        {
            _logger.LogInformation("Verificando existência de moto com placa {Plate} ou ID {MotorcycleId}", plate, id);

            try
            {
                var exists = await _db.Motorcycles.AnyAsync(m => m.Plate == plate || m.MotorcycleId == id);
                _logger.LogInformation("Existência verificada: {Exists}", exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar existência de moto com placa {Plate} ou ID {MotorcycleId}", plate, id);
                throw;
            }
        }

        public async Task<Motorcycle?> GetMotorcycleByPlateAsync(string plate)
        {
            _logger.LogInformation("Buscando moto pela placa {Plate}", plate);

            try
            {
                var motorcycle = await _db.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate);
                if (motorcycle is null)
                    _logger.LogWarning("Moto não encontrada com placa {Plate}", plate);
                else
                    _logger.LogInformation("Moto encontrada com placa {Plate}", plate);

                return motorcycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar moto pela placa {Plate}", plate);
                throw;
            }
        }

        public async Task UpdateAsync(Motorcycle existingMotorcycle)
        {
            _logger.LogInformation("Atualizando moto {MotorcycleId} com placa {Plate}", existingMotorcycle.MotorcycleId, existingMotorcycle.Plate);

            try
            {
                _db.Motorcycles.Update(existingMotorcycle);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Moto {MotorcycleId} atualizada com sucesso", existingMotorcycle.MotorcycleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar moto {MotorcycleId} com placa {Plate}", existingMotorcycle.MotorcycleId, existingMotorcycle.Plate);
                throw;
            }
        }

        public async Task<bool> ExistsByPlateAsync(string plate, string excludeId)
        {
            _logger.LogInformation("Verificando se placa {Plate} existe excluindo ID {ExcludeId}", plate, excludeId);

            try
            {
                var exists = await _db.Motorcycles.AnyAsync(m => m.Plate == plate && m.MotorcycleId != excludeId);
                _logger.LogInformation("Verificação de placa {Plate}: {Exists}", plate, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar placa {Plate} excluindo ID {ExcludeId}", plate, excludeId);
                throw;
            }
        }
    }
}
