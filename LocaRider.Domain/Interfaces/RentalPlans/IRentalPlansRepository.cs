using LocaRider.Domain.Entities.RentalPlan;

namespace LocaRider.Domain.Interfaces.RentalPlans
{
    public interface IRentalPlansRepository
    {
        Task<RentalPlan?> GetRentalPlanByPeriodInDaysAsync(int periodInDays);
    }
}
