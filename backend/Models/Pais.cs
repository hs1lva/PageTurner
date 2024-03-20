
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace backend.Models;
public class Pais
{
    //Atributos
    [Key]
    public int paisId { get; set; }
    public string nomePais { get; set; }

    private PageTurnerContext _Bd;//apenas para testes (falar com o professor)


    //Construtor
    public Pais(PageTurnerContext Bd) {
        _Bd = Bd;
     }
    public Pais(string nomePais, PageTurnerContext Bd)
    {
        _Bd = Bd;
        this.nomePais = nomePais;
    }


    //Metodos

    /// <summary>
    /// Apenas para testar e vermos como podemos organizar o código
    /// </summary>
    /// <returns></returns>
    public static async Task<List<Pais>> VerTodosPaises(PageTurnerContext Bd)
    {
        var lista = await Bd.Pais.ToListAsync();

        if (lista == null)
        {
            return null;
        }
        return lista;
    }



}

