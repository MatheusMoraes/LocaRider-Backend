using System.ComponentModel.DataAnnotations;

namespace LocaRider.Domain.Entities.Motorcycles
{
    public class Motorcycle
    {
        [Key]
        public string MotorcycleId { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public ICollection<LocaRider.Domain.Entities.Rental.Rental> Rentals { get; set; } = new List<LocaRider.Domain.Entities.Rental.Rental>();
    }

}
