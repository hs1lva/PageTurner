

using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class Pais
{
    [Key]
    public int paisId { get; set; }
    public string nomePais { get; set; }
}