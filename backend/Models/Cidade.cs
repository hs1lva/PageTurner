


using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class Cidade
{
    [Key]
    public int cidadeId { get; set; }
    public string nomeCidade { get; set; }

    //chave estranjeira para  pais
    public Pais paisCidade { get; set; }
}