using LocaRider.Domain.Entities.Driver;
using LocaRider.Domain.Interfaces.Drivers;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LocaRider.Infrastructure.Data.Repositories.Drivers
{
    public class DriversRepository : IDriversRepository
    {
        private readonly LocaRiderDbContext _db;
        public DriversRepository(LocaRiderDbContext db)
        {
            _db = db;
        }
        public async Task<Driver> AddAsync(Driver driver)
        {
            await _db.Drivers.AddAsync(driver);
            await _db.SaveChangesAsync();
            return driver;
        }

        public async Task<bool> ExistsByCNPJAsync(string cnpj)
        {
            return await _db.Drivers.AnyAsync(d => d.Cnpj == cnpj);
        }

        public async Task<bool> ExistsByCNHNumberAsync(string cnhNumber)
        {
            return await _db.Drivers.AnyAsync(d => d.CnhNumber == cnhNumber);
        }

        public Task<bool> AddBase64ImageAsync(string id, string base64Image)
        {
            throw new NotImplementedException();
        }

        public async Task<Driver?> GetByIdAsync(string id)
        {
            return await _db.Drivers.FirstOrDefaultAsync(d => d.DriverId == id);   
        }

        public async Task<Driver> UpdateAsync(Driver driver)
        {
            _db.Drivers.Update(driver);
            await _db.SaveChangesAsync();

            return driver;
        }
    }
}
