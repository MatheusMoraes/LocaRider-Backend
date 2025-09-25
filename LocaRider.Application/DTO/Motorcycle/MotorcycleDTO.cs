using System.ComponentModel.DataAnnotations;

namespace LocaRider.Application.DTO.Motorcycle
{
    public class MotorcycleDTO
    {
        [Required(ErrorMessage = "O identificador é obrigatório")]
        public string identificador { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano é obrigatório")]
        [Range(1900, 2100, ErrorMessage = "O ano deve estar entre 1900 e 2100")]
        public int ano { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório")]
        [StringLength(100, ErrorMessage = "O modelo deve ter no máximo 100 caracteres")]
        public string modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A placa é obrigatória")]
        public string placa { get; set; }
    }
}
