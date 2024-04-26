using System.ComponentModel.DataAnnotations;
namespace backend.Models;

public class UtilizadorCreateDTO
{
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
	public int tipoUtilizadorId { get; set; }
	public int estadoContaId { get; set; }
	public int cidadeId { get; set; }

}