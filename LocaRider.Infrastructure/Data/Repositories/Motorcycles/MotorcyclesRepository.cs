using LocaRider.Domain.Entities.Motorcycles;
using LocaRider.Domain.Interfaces.Motorcycles;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LocaRider.Infrastructure.Data.Repositories.Motorcycles
{
    public class MotorcyclesRepository : IMotorcyclesRepository
    {
        private readonly LocaRiderDbContext _db;

        public MotorcyclesRepository(LocaRiderDbContext db)
        {
            _db = db;
        }

        public async Task<Motorcycle> AddAsync(Motorcycle motorcycle)
        {
            _db.Motorcycles.Add(motorcycle);
            await _db.SaveChangesAsync();
            return motorcycle;
        }

        public async Task DeleteAsync(Motorcycle motorcycle)
        {
            _db.Motorcycles.Remove(motorcycle);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Motorcycle>> GetAllAsync()
        {
            return await _db.Motorcycles.AsNoTracking().ToListAsync();
        }

        public async Task<Motorcycle?> GetMotorcycleByIdAsync(string id)
        {
            return await _db.Motorcycles.FirstOrDefaultAsync(m => m.MotorcycleId == id);
        }

        public async Task<bool> VerifyExistsMotorcycleByPlateOrIdAsync(string plate, string id)
        {
            return await _db.Motorcycles
                .AnyAsync(m => m.Plate == plate || m.MotorcycleId == id);
        }

        public async Task<Motorcycle?> GetMotorcycleByPlateAsync(string plate)
        {
            return await _db.Motorcycles.FirstOrDefaultAsync(m => m.Plate == plate);
        }

        public async Task UpdateAsync(Motorcycle existingMotorcycle)
        {   
            _db.Motorcycles.Update(existingMotorcycle);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsByPlateAsync(string plate, string excludeId)
        {
            return await _db.Motorcycles
                .AnyAsync(m => m.Plate == plate && m.MotorcycleId != excludeId);
        }
    }
}
