using System.ComponentModel.DataAnnotations;

namespace DesafioBackEnd.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(12,MinimumLength = 4, ErrorMessage = "O nome de usuário deve conter no mínimo 4 caracteres e no máximo 12")]
        public string Username { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 6, ErrorMessage = "A senha deve conter entre 6 a 8 caracteres")]
        public string Password { get; set; }
    }
}