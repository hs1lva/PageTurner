
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;
public class Troca
{
    [Key]
    public int trocaId { get; set; }
    public DateTime dataPedidoTroca { get; set; }
    public DateTime? dataAceiteTroca { get; set; }
    public Estante estanteId2 { get; set; }
    public int estanteId { get; set; } //FK, não foi possivel definir o tipo como classe, porque já existe uma ligacao na BD com essa classe
    public EstadoTroca estadoTroca { get; set; }

    #region Construtores

    public Troca(){}

    public Troca(DateTime dataPedidoTroca, Estante estanteId2, int estanteId, 
                                    EstadoTroca estadoTroca)
    {
        this.dataPedidoTroca = dataPedidoTroca;
        this.estanteId2 = estanteId2;
        this.estanteId = estanteId;
        this.estadoTroca = estadoTroca;
    }
    #endregion

    /// <summary>
    /// Faz a procura de um livro em qql estante
    /// </summary>
    /// <param name="estanteOrigem"></param>
    /// <param name="estanteProcura"></param>
    /// <param name="livro"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ActionResult> ProcuraMatchEstante(int estanteOrigem, int estanteProcura, Livro livro, PageTurnerContext _bd)
    {   
        throw new NotImplementedException();
    }
}