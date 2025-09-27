using LocaRider.Domain.Entities.RentalPlan;
using LocaRider.Domain.Interfaces.RentalPlans;
using LocaRider.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LocaRider.Infrastructure.Data.Repositories.RentalPlans
{
    public class RentalPlansRepository : IRentalPlansRepository
    {
        private readonly LocaRiderDbContext _db;
        private readonly ILogger<RentalPlansRepository> _logger;

        public RentalPlansRepository(LocaRiderDbContext db, ILogger<RentalPlansRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<RentalPlan?> GetRentalPlanByPeriodInDaysAsync(int periodInDays)
        {
            _logger.LogInformation("Buscando plano de locação para período de {PeriodInDays} dias", periodInDays);

            try
            {
                var rentalPlan = await _db.RentalPlans.FirstOrDefaultAsync(rp => rp.RentalPlanPeriodInDays == periodInDays);

                if (rentalPlan is null)
                {
                    _logger.LogWarning("Plano de locação não encontrado para período de {PeriodInDays} dias", periodInDays);
                }
                else
                {
                    _logger.LogInformation("Plano de locação encontrado: {RentalPlanId} para {PeriodInDays} dias", rentalPlan.RentalPlanId, periodInDays);
                }

                return rentalPlan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar plano de locação para período de {PeriodInDays} dias", periodInDays);
                throw;
            }
        }
    }
}
