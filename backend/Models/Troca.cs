
using System.ComponentModel.DataAnnotations;
using backend.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;
public class Troca
{
    [Key]
    public int trocaId { get; set; }
    public DateTime dataPedidoTroca { get; set; }
    public DateTime? dataAceiteTroca { get; set; }
    public Estante estanteId2 { get; set; } //estante de quem solicita a troca!
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
        //ver se vale a pena juntar numa função só
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
    /// Procura numa lista de utilizadores se tem algum livro na minha estante
    /// </summary>
    /// <param name="listUser"></param>
    /// <param name="minhaEstante"></param>
    /// <param name="_bd"></param>
    /// <returns>Retorna uma lista de utilizadores que tem o livro</returns>
    public async Task<ActionResult<List<Utilizador>>> ProcuraLivroEmUtilizadores (List<Utilizador> listUser, Estante minhaEstante
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
    /// Solicita troca, envia email aos users, guarda a troca
    /// </summary>
    /// <param name="troca"></param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ActionResult<Troca?>> SolicitaTroca(Troca troca, PageTurnerContext _bd)
    {   // talvez seja melhor que a troca seja criada aqui dentro. Ver necessidades

        #region Verificações
        var validacao = await ValidaTroca(troca, _bd);

        var trocaValidada = validacao.Value.Item1;
        var utilizadorSolicitaTroca = validacao.Value.Item2;
        var utilizadorRecebeTroca = validacao.Value.Item3;

        if (trocaValidada == null)
        {
            throw new Exception("Troca não existe");
        }
        if (utilizadorSolicitaTroca == null || utilizadorRecebeTroca == null)
        {
            throw new Exception("Utilizador não existe");
        }

        #endregion

        //cria a troca, garante que a data é a atual e o estado é pendente
        troca.dataPedidoTroca = DateTime.Now;
        troca.dataAceiteTroca = null;
        var estado = await _bd.EstadoTroca
            .Where(x => x.descricaoEstadoTroca == "Pendente")
            .FirstOrDefaultAsync();
        if (estado == null)
        {
            throw new Exception("Estado não existe");// TODO -> Criar estado caso nao exista na bd
        }
        troca.estadoTroca = estado;


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

        return troca;
    }

    /// <summary>
    /// Cria uma troca, guarda a troca na bd
    /// </summary>
    /// <param name="userName">username do user que prretende a troca</param>
    /// <param name="estanteId">estante onde está o livro que o user quer</param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    public async Task<ActionResult<Troca?>> CriaTroca(string userName, int estanteId, PageTurnerContext _bd)
    {
        // procura o estado de troca pendente na BD 
        EstadoTroca estadoTroca = await EstadoTroca.ProcEstadoTroca("Pendente", _bd);

        // Procura a estante com o livro pretendido
        var estanteComLivro = await _bd.Estante
                            .Where(x => x.estanteId == estanteId)
                            .FirstOrDefaultAsync();

        if (estanteComLivro == null)
        {
            throw new Exception("Estante não existe");
        }

        //Cria troca
        var troca = new Troca(
            dataPedidoTroca: DateTime.Now,
            estanteId: estanteComLivro.estanteId,
            estanteId2: null, //ver se é preciso mudar a BD.
            estadoTroca: estadoTroca
        );

        if (troca == null)
        {
            throw new Exception("Troca não foi criada");
        }

        // Guarda a troca na bd
        try
        {
            _bd.Add(troca);
            await _bd.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return troca;
    }

    /// <summary>
    /// Troca direta, guarda na BD
    /// </summary>
    /// <param name="userName">User que solicita a troca</param>
    /// <param name="estanteId">Estante onde está o livro que o user quer</param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ActionResult<Troca?>> TrocaDireta(string userName, int estanteId, PageTurnerContext _bd)
    {
        //cria uma nova troca
        Troca troca = new Troca();
        var resultado = await troca.CriaTroca(userName, estanteId, _bd);
        if (resultado.Value != null)
        {
            Troca? trocaResult = resultado.Value;
            troca = trocaResult;
        }
        else
        {
            throw new Exception("Não foi possivel criar a troca");
        }

        //Procura a estante do livro pedido
        Estante? estanteDoLivroPedido = await _bd.Estante
                            .Where(x => x.estanteId == troca.estanteId)
                            .FirstOrDefaultAsync();
        if (estanteDoLivroPedido == null)
        {
            throw new Exception("Estante não existe");
        }
        //user que solicita a troca
        var user = await _bd.Utilizador
            .Where(x => x.username == userName)
            .FirstOrDefaultAsync();
        //user que recebe a troca
        var user2 = await _bd.Utilizador
            .Where(x => x.utilizadorID == estanteDoLivroPedido.utilizador.utilizadorID)
            .FirstOrDefaultAsync();
        if (user == null || user2 == null)
        {
            throw new Exception("Utilizador não existe");//não é suposto ter esta resposta, "trabalho academico".
        }

        //grava na bd
        try
        {
            _bd.Update(troca);
            await _bd.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        // envia email para o utilizador que solicita a troca
        await _emailSender.SendEmailAsync(user.email, "Solicitacão de troca",
                                        $"O pedido de troca foi efetuado com o numero: {troca.trocaId}");

        // envia email para o utilizador que recebe a troca
        await _emailSender.SendEmailAsync(user2.email, "Solicitacão de troca",
                                        "O pedido de troca foi efetuado com o numero: " + troca.trocaId);

        return troca;

    }

    /// <summary>
    /// Aceita troca, guarda na BD
    /// </summary>
    /// <param name="troca"></param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ActionResult<Troca?>> AceitaTroca(int trocaId, PageTurnerContext _bd)
    {
        var troca = await ProcTrocaByID(trocaId, _bd);

        #region Valida Troca
        var validacao = await ValidaTroca(troca, _bd);

        if (validacao.Value.Item1 != null)
        {
            throw new Exception("Troca não existe");
        }
        #endregion

        troca.dataAceiteTroca = DateTime.Now;
        var estado = await _bd.EstadoTroca
            .Where(x => x.descricaoEstadoTroca == "Aceite")
            .FirstOrDefaultAsync();
        if (estado == null)
        {
            throw new Exception("Estado não existe"); // TODO -> Criar estado caso nao exista na bd
        }
        troca.estadoTroca = estado;
        try
        {
            _bd.Update(troca);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
            
        }
        return troca;

    }

    /// <summary>
    /// Rejeita troca
    /// </summary>
    /// <param name="trocaID"></param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ActionResult<Troca>> RejeitaTroca(int trocaID, PageTurnerContext _bd)
    {
        //rejeita a troca.
        var troca = await ProcTrocaByID(trocaID, _bd);

        if (troca.estadoTroca.descricaoEstadoTroca == "Recusada")
        {
            throw new Exception("Troca já foi recusada");
        }
        
        if (troca == null)
        {
            throw new Exception("Troca não existe");
        }

        //atualiza estado da troca
        EstadoTroca estado = EstadoTroca.ProcEstadoTroca("Recusada", _bd).Result;
        troca.estadoTroca = estado;
        //garante que a data de aceite da troca seja nula, aka, não aceite
        troca.dataAceiteTroca = null;

        try
        {
            _bd.Update(troca);
            await _bd.SaveChangesAsync(); 
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return troca;
    }

    /// <summary>
    /// Procura, na BD, troca pelo numero do ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task<Troca?> ProcTrocaByID(int id, PageTurnerContext _bd)
    {

        Troca troca = await _bd.Troca
            .Where(x => x.trocaId == id)
            .FirstOrDefaultAsync();
        if (troca == null)
        {
            throw new Exception("Troca não existe");
        }

        return troca;

    }

    /// <summary>
    /// Valida troca
    /// </summary>
    /// <param name="troca"></param>
    /// <param name="_bd"></param>
    /// <returns>Retorna a troca, o utilizador que solicita a troca e o utilizador que recebe a troca</returns>
    /// <exception cref="Exception"></exception>
    private async Task<ActionResult<(Troca?, Utilizador?, Utilizador?)>> ValidaTroca(Troca troca, PageTurnerContext _bd){

        Utilizador? utilizadorSolicitaTroca = await _bd.Utilizador
                                    .Where(x => x.utilizadorID == troca.estanteId2.utilizador.utilizadorID)
                                    .FirstOrDefaultAsync();
        

        Estante? estante1 = await _bd.Estante
                                    .Where(x => x.estanteId == troca.estanteId)
                                    .FirstOrDefaultAsync();
        if (estante1 == null)
        {   return (null, null, null);
            throw new Exception("Estante não existe");
        }

        Utilizador? utilizadorRecebeTroca = await _bd.Utilizador
                                    .Where(x => x.utilizadorID == estante1.utilizador.utilizadorID)
                                    .FirstOrDefaultAsync();
        if (utilizadorRecebeTroca == null || utilizadorSolicitaTroca == null)
        {
            return (null, null, null);
            throw new Exception("Utilizador não existe");
        }
        //verifica se o utilizador tem a conta suspensa
        if(utilizadorSolicitaTroca.estadoConta.descricaoEstadoConta == "Suspensa" || 
            utilizadorRecebeTroca.estadoConta.descricaoEstadoConta == "Suspensa")
        {
            return (null, null, null);
            throw new Exception($"Utilizador não pode fazer trocas");
        }
        
        // verifica se o livro existe
        var livro = await _bd.Livro
            .Where(x => x.livroId == troca.estanteId2.livro.livroId)
            .FirstOrDefaultAsync();
        if (livro == null)
        {
            return ( null, null, null);
            throw new Exception("Livro não existe");
        }
        /// verifica se a estante existe
        var estante = await _bd.Estante
            .Where(x => x.estanteId == troca.estanteId)
            .FirstOrDefaultAsync();
        if (estante == null)
        {
            return ( null, null, null);
            throw new Exception("Estante não existe");
        }

        // verifica se troca já existe
        var trocaExistente = await _bd.Troca
            .Where(x => x.estanteId == troca.estanteId && x.estanteId2 == troca.estanteId2)
            .FirstOrDefaultAsync();

        if (trocaExistente != null)
        {
            return ( null, null, null);
            throw new Exception("Troca já existe");
        }
        

        return (troca, utilizadorSolicitaTroca, utilizadorRecebeTroca);

    }


}