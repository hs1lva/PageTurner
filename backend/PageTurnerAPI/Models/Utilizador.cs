using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using BCrypt.Net;

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
    [JsonIgnore]
    List<GeneroLivro> generosLivro { get; } = [];
    [JsonIgnore]
    List<AutorLivro> autoresLivro { get; } = [];

    //chave estranjeira para  tipoUtilizador
    public int tipoUtilizadorId { get; set; }
    // ignore no request
    [JsonIgnore]
    public TipoUtilizador tipoUtilizador { get; set; }

    //chave estranjeira para  estadoConta
    public int estadoContaId { get; set; }

    // Chave estrangeira para Cidade
    public int cidadeId { get; set; }
    
    // ignore no request
    [JsonIgnore]
    public EstadoConta estadoConta { get; set; }
    //chave estranjeira para  localizacao

    // Propriedade de navegação para as avaliações/comentarios
    public ICollection<AvaliacaoLivro> Avaliacoes { get; set; }
    public ICollection<ComentarioLivro> Comentarios { get; set; }

    // Construtor da classe Utilizador
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

    /// <summary>
    /// Atualizar a senha do utilizador
    /// </summary>
    /// <param name="novaSenha"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task AtualizarSenhaAsync(string novaSenha, PageTurnerContext context)
    {
        this.password = novaSenha;
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Verificar se o username já existe
    /// </summary>
    /// <param name="context"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public static bool UsernameExists(PageTurnerContext context, string username)
    {
        return context.Utilizador.Any(u => u.username == username);
    }

    /// <summary>
    /// Verificar se o email já existe
    /// </summary>
    /// <param name="context"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool EmailExists(PageTurnerContext context, string email)
    {
        return context.Utilizador.Any(u => u.email == email);
    }
}
