using System.ComponentModel.DataAnnotations;

namespace LocaRider.Application.DTO.Motorcycle
{
    public class MotorcyclePlateDTO
    {
        [Required(ErrorMessage = "A placa é obrigatória.")]
        public string placa { get; set; } = string.Empty;
    }
}
