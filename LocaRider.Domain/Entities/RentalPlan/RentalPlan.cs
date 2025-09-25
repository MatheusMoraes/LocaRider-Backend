namespace LocaRider.Domain.Entities.RentalPlan
{
    public class RentalPlan
    {
        public string RentalPlanId { get; set; } = string.Empty;
        public int  RentalPlanPeriodInDays { get; set; }
        public decimal DailyPrice { get; set; }
    }
}
