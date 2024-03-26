

using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Utilizador
{
    [Key]
    public int utilizadorID { get; set; }
    public string nome { get; set; }
    public string apelido { get; set; }
    public DateTime dataNascimento { get; set; }
    [Required]
    public string username { get; set; }
    [Required]
    public string password { get; set; }
    [Required]
    public string email { get; set; }
    public string fotoPerfil { get; set; }
    public DateTime? dataRegisto { get; set; }
    public DateTime ultimologin { get; set; }
    public bool notficacaoPedidoTroca { get; set; }
    public bool notficacaoAceiteTroca { get; set; }
    public bool notficacaoCorrespondencia { get; set; }

    //ligacao de muitos para muitos é feita desta forma
    List<GeneroLivro> generosLivro { get; } = [];
    List<AutorLivro> autoresLivro { get; } = [];

    //chave estranjeira para  tipoUtilizador
    public TipoUtilizador tipoUtilizador { get; set; }

    //chave estranjeira para  estadoConta
    public EstadoConta estadoConta { get; set; }
    //chave estranjeira para  localizacao

    // Propriedade de navegação para as avaliações/comentarios
    public ICollection<AvaliacaoLivro> Avaliacoes { get; set; }
	public ICollection<ComentarioLivro> Comentarios { get; set; }

    //Construtor
    public Utilizador()
    {
    }


    /// <summary>
    /// Atualizar o nome do utilizador
    /// </summary>
    /// <param name="novoNome"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task AtualizarNomeAsync(string novoNome, PageTurnerContext context)
    {
        this.nome = novoNome;
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualizar o apelido do utilizador
    /// </summary>
    /// <param name="novoApelido"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task AtualizarApelidoAsync(string novoApelido, PageTurnerContext context)
    {
        this.apelido = novoApelido;
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualizar a data de nascimento do utilizador
    /// </summary>
    /// <param name="novaDataNascimento"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task AtualizarDataNascimentoAsync(DateTime novaDataNascimento, PageTurnerContext context)
    {
        this.dataNascimento = novaDataNascimento;
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualizar a foto de perfil do utilizador
    /// </summary>
    /// <param name="novaFotoPerfil"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task AtualizarFotoPerfilAsync(string novaFotoPerfil, PageTurnerContext context)
    {
        this.fotoPerfil = novaFotoPerfil;
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualizar a data de registo do utilizador
    /// </summary>
    /// <param name="novaDataRegisto"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task AtualizarDataRegistoAsync(DateTime novaDataRegisto, PageTurnerContext context)
    {
        this.dataRegisto = novaDataRegisto;
        await context.SaveChangesAsync();
    }
}
