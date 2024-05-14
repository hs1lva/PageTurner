namespace backend.Models;


public class MatchDTO
{
    public List<Estante> ListaEstantesComLivroQueEuQuero { get; set; }
    public List<Utilizador> ListUsersTemLivroQueEuQuero { get; set; }
    public List<Estante> ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam { get; set; }
}