using System.ComponentModel.DataAnnotations;

namespace LocaRider.Application.DTO.Rental
{
    public class RentalDevolutionDTO
    {
        [Required(ErrorMessage = "A data de devolução é obrigatória")]
        public DateTime data_devolucao { get; set; }
    }
}
