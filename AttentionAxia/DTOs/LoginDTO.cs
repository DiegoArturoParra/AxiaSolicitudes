using System.ComponentModel.DataAnnotations;

namespace AttentionAxia.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El campo usuario es requerido.")]
        public string EmailOrNickName { get; set; }
        [Required(ErrorMessage = "El campo contraseña es requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}