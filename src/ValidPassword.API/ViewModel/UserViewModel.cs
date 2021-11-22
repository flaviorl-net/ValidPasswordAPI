using System.ComponentModel.DataAnnotations;

namespace ValidPassword.API.ViewModel
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "O nome do usuário deve ser informado")]
        [MaxLength(20, ErrorMessage = "Username deve ter no máximo de 20 carateres")]
        [MinLength(5, ErrorMessage = "Username deve ter no minimo de 5 carateres")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A senha deve ser informada")]
        [MaxLength(20, ErrorMessage = "Password deve ter no máximo de 20 carateres")]
        [MinLength(5, ErrorMessage = "Password deve ter no minimo de 5 carateres")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
