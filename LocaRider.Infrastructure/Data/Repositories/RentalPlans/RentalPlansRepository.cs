using LocaRider.Domain.Entities.RentalPlan;
using LocaRider.Domain.Interfaces.RentalPlans;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LocaRider.Infrastructure.Data.Repositories.RentalPlans
{
    public class RentalPlansRepository : IRentalPlansRepository
    {

        private readonly LocaRiderDbContext _db;

        public RentalPlansRepository(LocaRiderDbContext db)
        {
            _db = db;
        }

        public async Task<RentalPlan?> GetRentalPlanByPeriodInDaysAsync(int periodInDays)
        {
            return await _db.RentalPlans.FirstOrDefaultAsync(rp => rp.RentalPlanPeriodInDays == periodInDays);
        }
    }
}
