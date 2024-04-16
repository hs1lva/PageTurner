


using System.ComponentModel.DataAnnotations;

namespace backend.Models;
public class Cidade
{
    [Key]
    public int cidadeId { get; set; }
    public string nomeCidade { get; set; }
    private PageTurnerContext _Bd;//apenas para testes (falar com o professor)

    //chave estranjeira para Pais
    public int paisId { get; set; }

    //Construtor
    public Cidade(){

    }

    public Cidade(string nomeCidade, PageTurnerContext Bd)
    {
        _Bd = Bd;
        this.nomeCidade = nomeCidade;
    }

}