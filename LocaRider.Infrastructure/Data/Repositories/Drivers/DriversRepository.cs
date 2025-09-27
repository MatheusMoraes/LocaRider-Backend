using LocaRider.Domain.Entities.Driver;
using LocaRider.Domain.Interfaces.Drivers;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LocaRider.Infrastructure.Data.Repositories.Drivers
{
    public class DriversRepository : IDriversRepository
    {
        private readonly LocaRiderDbContext _db;
        private readonly ILogger<DriversRepository> _logger;

        public DriversRepository(LocaRiderDbContext db, ILogger<DriversRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Driver> AddAsync(Driver driver)
        {
            _logger.LogInformation("Adicionando novo motorista com ID {DriverId} e CNPJ {Cnpj}", driver.DriverId, driver.Cnpj);

            try
            {
                await _db.Drivers.AddAsync(driver);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Motorista {DriverId} adicionado com sucesso", driver.DriverId);
                return driver;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar motorista {DriverId} com CNPJ {Cnpj}", driver.DriverId, driver.Cnpj);
                throw;
            }
        }

        public async Task<bool> ExistsByCNPJAsync(string cnpj)
        {
            _logger.LogInformation("Verificando existência de motorista com CNPJ {Cnpj}", cnpj);
            try
            {
                var exists = await _db.Drivers.AnyAsync(d => d.Cnpj == cnpj);
                _logger.LogInformation("Existência de CNPJ {Cnpj}: {Exists}", cnpj, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar existência de motorista com CNPJ {Cnpj}", cnpj);
                throw;
            }
        }

        public async Task<bool> ExistsByCNHNumberAsync(string cnhNumber)
        {
            _logger.LogInformation("Verificando existência de motorista com CNH {CnhNumber}", cnhNumber);
            try
            {
                var exists = await _db.Drivers.AnyAsync(d => d.CnhNumber == cnhNumber);
                _logger.LogInformation("Existência de CNH {CnhNumber}: {Exists}", cnhNumber, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar existência de motorista com CNH {CnhNumber}", cnhNumber);
                throw;
            }
        }

        public Task<bool> AddBase64ImageAsync(string id, string base64Image)
        {
            _logger.LogWarning("Método AddBase64ImageAsync ainda não implementado. DriverId: {DriverId}", id);
            throw new NotImplementedException();
        }

        public async Task<Driver?> GetByIdAsync(string id)
        {
            _logger.LogInformation("Buscando motorista pelo ID {DriverId}", id);
            try
            {
                var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.DriverId == id);

                if (driver is null)
                    _logger.LogWarning("Motorista não encontrado. ID {DriverId}", id);
                else
                    _logger.LogInformation("Motorista encontrado. ID {DriverId}", id);

                return driver;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar motorista pelo ID {DriverId}", id);
                throw;
            }
        }

        public async Task<Driver> UpdateAsync(Driver driver)
        {
            _logger.LogInformation("Atualizando motorista {DriverId}", driver.DriverId);
            try
            {
                _db.Drivers.Update(driver);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Motorista {DriverId} atualizado com sucesso", driver.DriverId);
                return driver;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar motorista {DriverId}", driver.DriverId);
                throw;
            }
        }
    }
}
