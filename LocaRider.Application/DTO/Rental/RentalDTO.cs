using System.ComponentModel.DataAnnotations;

namespace LocaRider.Application.DTO.Rental
{
    public class RentalDTO
    {

        [Required(ErrorMessage = "O ID do entregador é obrigatório")]
        public string entregador_id { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID da motocicleta é obrigatório")]
        public string moto_id { get; set; } = string.Empty;

        public decimal? valor_diaria { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória")]
        public DateTime data_inicio { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória")]
        public DateTime data_termino { get; set; }

        [Required(ErrorMessage = "A data prevista de término é obrigatória")]
        public DateTime data_previsao_termino { get; set; }

        [Required(ErrorMessage = "O plano é obrigatório")]
        [Range(1, 50, ErrorMessage = "Plano inválido")]
        public int plano { get; set; }

    }
}
