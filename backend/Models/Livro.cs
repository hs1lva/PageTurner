using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;
public class Livro
{
    [Key]
    public int livroId { get; set; }
    public string tituloLivro { get; set; }
    public int anoPrimeiraPublicacao { get; set; }
    public string idiomaOriginalLivro { get; set; }

    //chave estranjeira para  autorLivro
    public AutorLivro autorLivro { get; set; }
    //chave estranjeira para  generoLivro
    public GeneroLivro generoLivro { get; set; }

    // Propriedade de navegação para as avaliações
    public ICollection<AvaliacaoLivro> Avaliacoes { get; set; }
    public ICollection<ComentarioLivro> Comentarios { get; set; }

    // issue #66
    public double? MediaAvaliacao()
    {
        if (Avaliacoes == null || !Avaliacoes.Any())
        {
            return null;
        }

        return Avaliacoes.Average(a => a.Nota);
    }

    /// <summary>
    /// Metodo para sugerir livros com base nos livros da estante do utilizador
    /// </summary>
    /// <param name="utilizadorId"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    // Método para sugerir livros com base nos livros da estante do utilizador
    public async Task<List<LivroDTO>> SugerirLivros(int utilizadorId, PageTurnerContext context)
    {
        try
        {
            // Recuperar os livros da estante do utilizador com base no tipo da estante
            var livrosDaEstante = await context.Estante
                .Include(e => e.livro)
                    .ThenInclude(l => l.autorLivro)
                .Include(e => e.livro)
                    .ThenInclude(l => l.generoLivro)
                .Where(e => e.utilizador.utilizadorID == utilizadorId)
                .Select(e => e.livro)
                .ToListAsync();

            // Lista de Autores e Géneros dos livros da estante do utilizador
            var autoresEstante = livrosDaEstante.Select(l => l.autorLivro).ToList();
            var generosEstante = livrosDaEstante.Select(l => l.generoLivro).ToList();

            var livrosSugeridos = await context.Livro
                .Include(l => l.autorLivro)
                .Include(l => l.generoLivro)
                .Where(l => autoresEstante.Contains(l.autorLivro) || generosEstante.Contains(l.generoLivro))
                .ToListAsync();

            // Comparar as duas listas e retirar os duplicados da lista de livros sugeridos
            // Comparar duplicados dos livrosDaEstante e livrosSugeridos
            // Remover o livro que o utilizador já tem na estante
            var a = livrosSugeridos.Where(l => !livrosDaEstante.Contains(l));

            livrosSugeridos = a.ToList();

            // Mapear os livros sugeridos para LivroDTO
            var livrosSugeridosDTO = livrosSugeridos.Select(l => new LivroDTO
            {
                LivroId = l.livroId,
                TituloLivro = l.tituloLivro,
                AnoPrimeiraPublicacao = l.anoPrimeiraPublicacao,
                IdiomaOriginalLivro = l.idiomaOriginalLivro,
                AutorLivro = l.autorLivro,
                GeneroLivro = l.generoLivro,
                MediaAvaliacao = l.MediaAvaliacao(),
                Comentarios = l.Comentarios,
                Avaliacoes = l.Avaliacoes
            }).ToList();

            return livrosSugeridosDTO;
        }
        catch (Exception ex)
        {
            // Lidar com a exceção, como logá-la ou relançá-la
            throw new Exception("Erro ao sugerir livros.", ex);
        }
    }
}
