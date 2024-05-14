using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;
public class AutorLivro
{
    [Key]
    public int autorLivroId { get; set; }
    public string nomeAutorNome { get; set; }

    List<Utilizador> utilizadores { get; } = [];

    // Verificar se o autor existe pelo nome
    public static int VerificarAutor(string nomeAutor, PageTurnerContext context)
    {
        if (context.AutorLivro.Any(a => a.nomeAutorNome.ToLower() == nomeAutor.ToLower()))
        {
            return context.AutorLivro.FirstOrDefault(a => a.nomeAutorNome.ToLower() == nomeAutor.ToLower()).autorLivroId;
        }

        return 0;
    }
}
