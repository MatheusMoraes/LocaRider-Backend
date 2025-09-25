using System.ComponentModel.DataAnnotations;

namespace LocaRider.Application.DTO.Driver
{
    public class DriverBase64ImageDTO
    {
        [Required(ErrorMessage = "A imagem da CNH é obrigatória.")]
        public string imagem_cnh { get; set; } = string.Empty;
    }
}
