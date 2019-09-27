using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore_2_2_JWT.Models
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage ="O Campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O Campo {0} está em formato inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        [StringLength(100,ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres",MinimumLength = 6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage ="As senha não conferem")]
        public string ConfirmPassword { get; set; }
    }
}
