using System.ComponentModel.DataAnnotations;

namespace AppCitas.Service.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]  //El campo siguiente es requerido, no nullable o algo así
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
