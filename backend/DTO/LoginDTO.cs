using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class LoginDTO
    {
    [Required(ErrorMessage = "O campo Username é obrigatório.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "O campo Password é obrigatório.")]
    public string Password { get; set; }
    }

}