using System.ComponentModel.DataAnnotations;

namespace LocaRider.Application.DTO.Users
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Password { get; set; } = null!;
    }
}
