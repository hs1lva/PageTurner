
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

    private PageTurnerContext _context;//apenas para testes (falar com o professor)


    //Construtor
    public Pais(PageTurnerContext context) {
        _context = context;
     }
    public Pais(string nomePais, PageTurnerContext context)
    {
        _context = context;
        this.nomePais = nomePais;
    }


    //Metodos

    /// <summary>
    /// Apenas para testar e vermos como podemos organizar o código
    /// </summary>
    /// <returns></returns>
    public async Task<List<Pais>> VerTodosPaises()
    {
        var lista = await _context.Pais.ToListAsync();

        if (lista == null)
        {
            return null;
        }
        return lista;
    }


}

