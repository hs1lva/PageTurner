

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
    public DateTime dataRegisto { get; set; }
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
    public Cidade cidade { get; set; }

}
