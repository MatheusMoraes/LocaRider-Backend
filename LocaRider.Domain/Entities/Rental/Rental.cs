using LocaRider.Domain.Entities.Motorcycles;
using System.ComponentModel.DataAnnotations;

namespace LocaRider.Domain.Entities.Rental
{
    public class Rental
    {
        [Key]
        public string RentalId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;

        public string MotorcycleId { get; set; } = string.Empty;
        public Motorcycle Motorcycle { get; set; } = null!;

        public LocaRider.Domain.Entities.Driver.Driver Driver { get; set; } = null!;

        public decimal DailyPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public DateTime? DevolutionDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public int Plan { get; set; }
    }
}
