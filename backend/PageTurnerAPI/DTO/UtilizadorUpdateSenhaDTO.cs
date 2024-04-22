using System.ComponentModel.DataAnnotations;
namespace backend.Models;

public class UtilizadorUpdateSenhaDTO
{
    [Required]
    public string NovaSenha { get; set; }
}