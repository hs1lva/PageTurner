using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

    private readonly PageTurnerContext _context;

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

    // Construtor da classe Utilizador com o contexto da base de dados
    public Utilizador(PageTurnerContext context)
    {
        _context = context;
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
    /// Verificar se o userId já existe
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userid"></param>
    /// <returns></returns>
    public static bool UserIdExists(PageTurnerContext context, int utilizadorId)
    {
        return context.Utilizador.Any(u => u.utilizadorID == utilizadorId);
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

    /// <summary>
    /// Hash da senha do utilizador
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string HashPassword(string password)
    {
        // Defina o custo do hash (quanto maior, mais seguro, mas também mais lento)
        int workFactor = 12; // Ajuste conforme necessário

        // Gerar o hash da senha usando bcrypt
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
    }

    /// <summary>
    /// Verificar se um utilizador existe
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool UtilizadorExists(PageTurnerContext context, int id)
    {
        return context.Utilizador.Any(e => e.utilizadorID == id);
    }

    /// <summary>
    /// Função para confirmar o registo do utilizador
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task ConfirmarRegistoAsync(PageTurnerContext context)
    {
        this.dataRegisto = DateTime.Now;
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Funcao para obter o username do utilizador pelo id
    /// </summary>
    /// <param name="context"></param>
    /// <param name="id"></param>
    public static string GetUsernameById(PageTurnerContext context, int id)
    {
        return context.Utilizador.Where(u => u.utilizadorID == id).FirstOrDefault().username;
    }

    /// <summary>
    /// Verifica se o utilizador está verificado com base na data de registo não nula.
    /// </summary>
    /// <param name="userId">O ID do utilizador a ser verificado.</param>
    /// <param name="context">O contexto do banco de dados.</param>
    /// <returns>True se o utilizador estiver verificado, False caso contrário.</returns>
    public static async Task<bool> IsUserVerifiedAsync(int userId, PageTurnerContext context)
    {
        var user = await context.Utilizador.FindAsync(userId);
        return user != null && user.dataRegisto != null;
    }

    /// <summary>
    /// Obter os países dos utilizadores
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<string>> ObterPaisesUtilizadores(PageTurnerContext context)
    {
        // Realizar uma junção entre as tabelas Utilizador, Cidade e Pais para obter os países dos utilizadores
        var paisesUtilizadores = context.Utilizador
            .Join(context.Cidade, u => u.cidadeId, c => c.cidadeId, (u, c) => new { u, c })
            .Join(context.Pais, cu => cu.c.paisId, p => p.paisId, (cu, p) => p.nomePais)
            .Distinct() // Garantir que cada país aparece apenas uma vez na lista
            .ToList();

        return paisesUtilizadores;
    }

    public async Task BanirUtilizador()
    {
        // Pesquisar o estado "Banido" na bd
        var estadoBanido = await _context.EstadoConta.FirstOrDefaultAsync(e => e.descricaoEstadoConta == "Banido");

        // Verificar se o estado "Banido" foi encontrado
        try
        {
            // Atualizar o estado do user para o estado "Banido"
            this.estadoContaId = estadoBanido.estadoContaId;
            this.estadoConta = estadoBanido;

            // Executar as ações de banimento específicas
            // * Invalidar tokens de autenticação do usuário
            // * Enviar notificação por email ao usuário informando o banimento

            await _context.SaveChangesAsync();
        }

        catch
        {
            throw new Exception("Estado banido não encontrado.");
        }
    }

    /// <summary>
    /// Obter os utilizadores pelo login DTO
    /// </summary>
    /// <param name="loginDTO"></param>
    /// <param name="_context"></param>
    /// <returns></returns>
    public static async Task<Utilizador> GetUtilizadorByLoginDTO(LoginDTO loginDTO, PageTurnerContext _context)
    {
        var user = await _context.Utilizador
            .Include(u => u.tipoUtilizador)
            .FirstOrDefaultAsync(u => u.username == loginDTO.Username);
        if (user == null)
        {
            return null; // Utilizador não encontrado
        }
        return user;
    }

    /// <summary>
    /// Verificar se a senha corresponde ao hash
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    public static Task<bool> CheckPassword(string password, string hash)
    {
        bool confere = BCrypt.Net.BCrypt.Verify(password, hash);
        return Task.FromResult(confere);
    }

    /// <summary>
    /// Função para fazer login
    /// </summary>
    /// <param name="loginDTO"></param>
    /// <param name="_context"></param>
    /// <returns></returns>
    public static async Task<string> Login(Utilizador user)
    {

        var claims = new List<Claim>();

        if (user.tipoUtilizador.descricaoTipoUti == "Administrador") // Fazer enums para isto
            claims.Add(new Claim("user_type", "Admin"));
        //passar isto para o modelo
        claims.Add(new Claim("user_id", user.utilizadorID.ToString()));
        claims.Add(new Claim("user_name", user.username));
        claims.Add(new Claim("user_email", user.email));
        JwtAuth jwtTokenGenerator = new JwtAuth("PDS2024$3cr3tK3y@Jwt#2024PageTurnerAPI");
        var token = jwtTokenGenerator.GenerateJwtToken(claims);

        return token;
    }
}
