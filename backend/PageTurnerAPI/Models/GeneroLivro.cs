using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class GeneroLivro
{
    [Key]
    public int generoId { get; set; }
    public string descricaoGenero { get; set; }
    List<Utilizador> utilizadores { get; } = [];


    // Verificar se o género existe pela descrição
    public static int VerificarGenero(string descricaoGenero, PageTurnerContext context)
    {
        if (context.GeneroLivro.Any(g => g.descricaoGenero.ToLower() == descricaoGenero.ToLower()))
        {
            return context.GeneroLivro.FirstOrDefault(g => g.descricaoGenero.ToLower() == descricaoGenero.ToLower()).generoId;
        }       
        return 0;
    }
}