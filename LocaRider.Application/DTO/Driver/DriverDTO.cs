using System.ComponentModel.DataAnnotations;

namespace LocaRider.Application.DTO.Driver
{
    public class DriverDTO
    {
        [Required]
        public string identificador { get; set; } = string.Empty;

        [Required]
        public string nome { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"\d{14}", ErrorMessage = "CNPJ deve ter 14 dígitos")]
        public string cnpj { get; set; } = string.Empty;

        [Required]
        public DateTime data_nascimento { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Número da CNH deve ter 11 caracteres")]
        public string numero_cnh { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(A|B|A\+B)$", ErrorMessage = "Tipo de CNH deve ser A, B ou A+B")]
        public string tipo_cnh { get; set; } = string.Empty;

        [Required]
        public string imagem_cnh { get; set; } = string.Empty;
    }
}
