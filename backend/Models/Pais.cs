
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;


namespace backend.Models;
public class Pais
{
    //Atributos
    [Key]
    public int paisId { get; set; }
    public string nomePais { get; set; }

    private PageTurnerContext _context;
    //Construtor
    public Pais(string nomePais, PageTurnerContext context)
    {
        _context = context;
        this.nomePais = nomePais;
    }

}

