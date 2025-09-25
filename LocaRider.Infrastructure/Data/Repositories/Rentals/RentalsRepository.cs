using LocaRider.Domain.Entities.Rental;
using LocaRider.Domain.Interfaces.Rentals;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace LocaRider.Infrastructure.Data.Repositories.Rentals
{
    public class RentalsRepository : IRentalsRepository
    {
        private readonly LocaRiderDbContext _db;

        public RentalsRepository(LocaRiderDbContext db)
        {
            _db = db;
        }

        public async Task<Rental?> GetByIdAsync(string rentalId)
        {
            return await _db.Rentals.FirstOrDefaultAsync(r => r.RentalId == rentalId);
        }

        public async Task AddAsync(Rental rental)
        {
            await _db.Rentals.AddAsync(rental);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rental rental)
        {
            _db.Rentals.Update(rental);
            await _db.SaveChangesAsync();
        }

        public async Task<Rental?> GetActiveRentalByMotorcycleAsync(string motorcycleId)
        {
            return await _db.Rentals
                .Where(r => r.MotorcycleId == motorcycleId && r.DevolutionDate == default(DateTime))
                .FirstOrDefaultAsync();
        }
    }
}
