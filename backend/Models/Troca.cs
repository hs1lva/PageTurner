
using System.ComponentModel.DataAnnotations;
using backend.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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
    private IEmailSender _emailSender;

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

    /// <summary>
    /// Faz a procura de um livro em qql estante
    /// </summary>
    /// <param name="livroId"></param>
    /// <param name="estanteId"></param>
    /// <param name="_bd"></param>
    /// <returns>Retorna lista de utilizadores</returns>
        public async Task<ActionResult<List<Utilizador>>> ProcuraLivroEmEstante(int livroId, string estanteProcura, PageTurnerContext _bd)
        {
            //verifica se o livro existe na estante
            //procura livro na estante, se existir devolve ou os utilizadores 
            //temos de corrigir a base de dados primeiro.
            List<Estante> resp = await _bd.Estante
                .Include(x => x.tipoEstante)
                .Include(x => x.utilizador)
                .Where(x => x.tipoEstante.descricaoTipoEstante == estanteProcura && 
                            x.livro.livroId == livroId)
                .ToListAsync();
            
            //converte a lista de estantes em uma lista de utilizadores
            List<Utilizador> listUser = new List<Utilizador>();
            foreach (var item in resp)
            {
                listUser.Add(item.utilizador);
            }
            return listUser;
        }

        /// <summary>
        /// Procura num lista de utilizadores se tem algum livro na minha estante
        /// </summary>
        /// <param name="listUser"></param>
        /// <param name="minhaEstante"></param>
        /// <param name="_bd"></param>
        /// <returns>Retorna uma lista de utilizadores que tem o livro</returns>
        public async Task<ActionResult<List<Utilizador>>> ProcuraEstanteEm (List<Utilizador> listUser, Estante minhaEstante
                                                            , string estanteProcura, PageTurnerContext _bd)
        {   
            //cria nova lista de utilizadores
            List<Utilizador> novaLista = new List<Utilizador>();

            //percorre a lista de utilizadores
            foreach (var user in listUser)
            {
                // verifica, gurda 
                var estantes = await _bd.Estante
                    .Include(x => x.tipoEstante)
                    .Include(x => x.utilizador)
                    .Where(x => x.tipoEstante.descricaoTipoEstante == estanteProcura && 
                                x.utilizador.utilizadorID == user.utilizadorID)
                    .ToListAsync();

                if (estantes.Count != 0)
                {
                    foreach (var item in estantes)
                    {
                        if (item.livro.livroId == minhaEstante.livro.livroId)
                        {
                            novaLista.Add(item.utilizador);
                            break;// mesmo que tenha mais do que um livro, adiciona o user apenas uma vez
                        }
                    }
                }
            }
            
            return novaLista;
        }

        /// <summary>
        /// Solicita troca, a um utilizador
        /// </summary>
        /// <param name="troca"></param>
        /// <param name="_bd"></param>
        /// <returns>boolean</returns>
        /// <exception cref="Exception"></exception>
        public async Task<ActionResult<bool>> SolicitaTroca(Troca troca, PageTurnerContext _bd)
        {   
            #region Verificações
            //verifica se o utilizador existe
            Utilizador? utilizadorSolicitaTroca = await _bd.Utilizador
                                        .Where(x => x.utilizadorID == troca.estanteId2.utilizador.utilizadorID)
                                        .FirstOrDefaultAsync();
            

            Estante? estante1 = await _bd.Estante
                                        .Where(x => x.estanteId == troca.estanteId)
                                        .FirstOrDefaultAsync();

            Utilizador? utilizadorRecebeTroca = await _bd.Utilizador
                                        .Where(x => x.utilizadorID == estante1.utilizador.utilizadorID)
                                        .FirstOrDefaultAsync();
            if (utilizadorRecebeTroca == null || utilizadorSolicitaTroca == null || estante1 == null)
            {
                throw new Exception("Utilizador não existe");
            }
            //verifica se o utilizador tem a conta suspensa
            if(utilizadorSolicitaTroca.estadoConta.descricaoEstadoConta == "Suspensa" || 
                utilizadorRecebeTroca.estadoConta.descricaoEstadoConta == "Suspensa")
            {
                throw new Exception($"Utilizador não pode fazer trocas");
            }
            
            // verifica se o livro existe
            var livro = await _bd.Livro
                .Where(x => x.livroId == troca.estanteId2.livro.livroId)
                .FirstOrDefaultAsync();
            if (livro == null)
            {
                throw new Exception("Livro não existe");
            }
            /// verifica se a estante existe
            var estante = await _bd.Estante
                .Where(x => x.estanteId == troca.estanteId)
                .FirstOrDefaultAsync();
            if (estante == null)
            {
                throw new Exception("Estante não existe");
            }

            // verifica se troca já existe
            var trocaExistente = await _bd.Troca
                .Where(x => x.estanteId == troca.estanteId && x.estanteId2 == troca.estanteId2)
                .FirstOrDefaultAsync();

            if (trocaExistente != null)
            {
                throw new Exception("Troca já existe");
            }
            

            #endregion


            //cria a troca, garante que a data é a atual e o estado é pendente
            troca.dataPedidoTroca = DateTime.Now;
            troca.dataAceiteTroca = null;
            troca.estadoTroca = await _bd.EstadoTroca
                .Where(x => x.descricaoEstadoTroca == "Pendente")
                .FirstOrDefaultAsync();


            try
            {
                //envia o email para o utilizador que solicitou a troca
                await _emailSender.SendEmailAsync(utilizadorSolicitaTroca.email, "Solicitacão de troca",
                                                "O pedido de troca foi efetuado");            
                //envia o email para o utilizador para o utilizador que recebeu a troca
                await _emailSender.SendEmailAsync(utilizadorRecebeTroca.email, "Solicitacão de troca",
                                                "O pedido de troca foi efetuado");   
                //adiciona a troca na bd
                _bd.Add(troca);
                await _bd.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return true;
        }




}