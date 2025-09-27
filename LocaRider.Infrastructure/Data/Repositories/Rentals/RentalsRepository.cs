using LocaRider.Domain.Entities.Rental;
using LocaRider.Domain.Interfaces.Rentals;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocaRider.Infrastructure.Data.Repositories.Rentals
{
    public class RentalsRepository : IRentalsRepository
    {
        private readonly LocaRiderDbContext _db;
        private readonly ILogger<RentalsRepository> _logger;

        public RentalsRepository(LocaRiderDbContext db, ILogger<RentalsRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Rental?> GetByIdAsync(string rentalId)
        {
            _logger.LogInformation("Buscando locação com ID {RentalId}", rentalId);

            try
            {
                var rental = await _db.Rentals.FirstOrDefaultAsync(r => r.RentalId == rentalId);

                if (rental is null)
                    _logger.LogWarning("Locação não encontrada para ID {RentalId}", rentalId);
                else
                    _logger.LogInformation("Locação encontrada: {RentalId}", rental.RentalId);

                return rental;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar locação com ID {RentalId}", rentalId);
                throw;
            }
        }

        public async Task AddAsync(Rental rental)
        {
            _logger.LogInformation("Adicionando nova locação para moto {MotorcycleId}", rental.MotorcycleId);

            try
            {
                await _db.Rentals.AddAsync(rental);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Locação adicionada com sucesso: {RentalId}", rental.RentalId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar locação para moto {MotorcycleId}", rental.MotorcycleId);
                throw;
            }
        }

        public async Task UpdateAsync(Rental rental)
        {
            _logger.LogInformation("Atualizando locação {RentalId}", rental.RentalId);

            try
            {
                _db.Rentals.Update(rental);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Locação {RentalId} atualizada com sucesso", rental.RentalId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar locação {RentalId}", rental.RentalId);
                throw;
            }
        }

        public async Task<Rental?> GetActiveRentalByMotorcycleAsync(string motorcycleId)
        {
            _logger.LogInformation("Buscando locação ativa para moto {MotorcycleId}", motorcycleId);

            try
            {
                var rental = await _db.Rentals
                    .Where(r => r.MotorcycleId == motorcycleId && r.DevolutionDate == default(DateTime))
                    .FirstOrDefaultAsync();

                if (rental is null)
                    _logger.LogWarning("Nenhuma locação ativa encontrada para moto {MotorcycleId}", motorcycleId);
                else
                    _logger.LogInformation("Locação ativa encontrada: {RentalId} para moto {MotorcycleId}", rental.RentalId, motorcycleId);

                return rental;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar locação ativa para moto {MotorcycleId}", motorcycleId);
                throw;
            }
        }
    }
}
