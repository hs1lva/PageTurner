using System.ComponentModel.DataAnnotations;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;
public class Estante
{
    [Key]
    public int estanteId { get; set; }
    public DateTime ultimaAtualizacao { get; set; }
    //chave estrangeira para  tipoEstante
    public TipoEstante tipoEstante { get; set; }
    //chave estrangeira para  utilizador
    public Utilizador utilizador { get; set; }
    //chave estrangeira para livro
    public Livro livro { get; set; }
    //verificar se o livro está na estante ou se foi excluído
    public bool livroNaEstante { get; set; }

    #region Lógica
    /// <summary>
    /// Método para verificar se a estante existe na base de dados pelo id
    /// </summary>
    /// <param name="context"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool EstanteExists(PageTurnerContext context, int id)
    {
        return context.Estante.Any(e => e.estanteId == id);
    }

    /// <summary>
    /// Método para verificar na base de dados se existe uma estante com o livro, utilizador e tipo especificados e retorná-la
    /// </summary>
    /// <param name="context"></param>
    /// <param name="livroId"></param>
    /// <param name="utilizadorId"></param>
    /// <param name="tipoEstanteId"></param>
    /// <returns></returns>
    public static async Task<Estante> ObterEstanteExistente(PageTurnerContext context, int livroId, int utilizadorId, int tipoEstanteId)
    {
        var estanteExistente = await context.Estante
            .FirstOrDefaultAsync(e => e.livro.livroId == livroId
                                    && e.utilizador.utilizadorID == utilizadorId
                                    && e.tipoEstante.tipoEstanteId == tipoEstanteId);

        return estanteExistente;
    }

    /// <summary>
    /// Método para pesquisar a estante na base de dados
    /// </summary>
    /// <param name="_context"></param>
    /// <returns></returns>
    public static async Task<List<EstanteViewDTO>> PesquisaEstanteBD(PageTurnerContext _context)
    {
        var listaEstantes = await _context.Estante
                .Include(e => e.livro)
                .Include(e => e.livro.generoLivro)
                .Include(e => e.livro.autorLivro)
                .Include(e => e.tipoEstante)
                .Select(e => new EstanteViewDTO
                {
                    estanteId = e.estanteId,
                    ultimaAtualizacao = e.ultimaAtualizacao,
                    tipoEstante = e.tipoEstante,
                    livro = e.livro,
                    livroNaEstante = e.livroNaEstante,
                    utilizadorId = e.utilizador.utilizadorID,
                    username = e.utilizador.username
                }).ToListAsync();

        return listaEstantes;
    }

    /// <summary>
    /// Método para atualizar a estante
    /// </summary>
    /// <param name="context"></param>
    /// <param name="estanteId"></param>
    /// <param name="estante"></param>
    /// <returns></returns>
    public static async Task<bool> AtualizarEstanteBD(PageTurnerContext context, int estanteId, EstanteUpdateDTO estante)
    {
        var estanteAtual = await context.Estante.FindAsync(estanteId);
        if (estanteAtual == null)
        {
            return false; // Indica que a estante não foi encontrada
        }

        estanteAtual.ultimaAtualizacao = DateTime.Now;
        estanteAtual.tipoEstante = await context.TipoEstante.FindAsync(estante.tipoEstanteId);
        estanteAtual.livro = await context.Livro.FindAsync(estante.livroId);
        estanteAtual.livroNaEstante = estante.livroNaEstante;

        try
        {
            await context.SaveChangesAsync();
            return true; // Indica que a estante foi atualizada com sucesso
        }
        catch (DbUpdateConcurrencyException)
        {
            return false; // Indica que ocorreu um erro ao atualizar a estante
        }
    }

    /// <summary>
    /// Método para alterar o estado da estante (livro na estante ou excluído)
    /// </summary>
    /// <param name="context"></param>
    /// <param name="estanteId"></param>
    /// <returns></returns>
    public static async Task<bool> AlterarEstadoEstanteBD(PageTurnerContext context, int estanteId)
    {
        var estanteAtual = await context.Estante.FindAsync(estanteId);
        if (estanteAtual == null)
        {
            return false; // Indica que a estante não foi encontrada
        }

        // Inverte o estado do livro na estante
        estanteAtual.livroNaEstante = !estanteAtual.livroNaEstante;
        estanteAtual.ultimaAtualizacao = DateTime.Now;

        try
        {
            await context.SaveChangesAsync();
            return true; // Indica que o estado da estante foi alterado com sucesso
        }
        catch (DbUpdateConcurrencyException)
        {
            return false; // Indica que ocorreu um erro ao alterar o estado da estante
        }
    }

    /// <summary>
    /// Método para criar uma nova estante
    /// </summary>
    /// <param name="context"></param>
    /// <param name="estante"></param>
    /// <returns></returns>
    public static async Task<Estante> CriarNovaEstante(PageTurnerContext context, EstanteSubmitDTO estante)
    {
        var novaEstante = new Estante
        {
            ultimaAtualizacao = DateTime.Now,
            tipoEstante = await context.TipoEstante.FindAsync(estante.tipoEstanteId),
            utilizador = await context.Utilizador.FindAsync(estante.utilizadorId),
            livro = await context.Livro.FindAsync(estante.livroId),
            livroNaEstante = true
        };

        if (novaEstante == null)
        {
            return null; // Se a nova estante não puder ser criada, retorna null
        }

        context.Estante.Add(novaEstante);
        await context.SaveChangesAsync();

        return novaEstante; // Retorna a nova estante criada
    }

    /// <summary>
    /// Método para excluir a estante através do seu ID
    /// </summary>
    /// <param name="context"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static async Task<bool> ExcluirEstantePorId(PageTurnerContext context, int id)
    {
        var estante = await context.Estante.FindAsync(id);
        if (estante == null)
        {
            return false; // Indica que a estante não foi encontrada
        }

        context.Estante.Remove(estante);
        await context.SaveChangesAsync();

        return true; // Indica que a estante foi excluída com sucesso
    }

    #endregion
}