
using System.ComponentModel.DataAnnotations;
using backend.Controllers;
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
    private readonly PageTurnerContext _bd;

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
    /// Trata do math entre estantes
    /// Caso seja adicionado um livro na estante de troca, procura match
    /// </summary>
    /// <param name="minhaEstanteId">ID da estante onde é feita a adição do livro</param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ActionResult<MatchDTO>> ProcuraMatch(int minhaEstanteId, PageTurnerContext _bd)
    {   
        // Esta função será chamada sempre que um utilizador adiciona um livro na estante de troca ou de desejos.
        // Condições para ter match
        // 1. O Utilizador tem de ter pelo menos uma estante de troca e uma estante de desejo.

        // Procura se a estante é de se desejo ou de troca
        // #TODO -> Mudar para função das estantes
        var minhaEstante = await _bd.Estante
            .Include(x => x.tipoEstante)
            .Include(x => x.livro)
            .Include(x => x.utilizador)
            .Where(x => x.estanteId == minhaEstanteId)
            .FirstOrDefaultAsync();

        _ = minhaEstante ?? throw new Exception("Estante não existe");
        
        if(minhaEstante.tipoEstante.descricaoTipoEstante == "Estante Desejos"){ // Verifica se o tipo de estante é de desejo
            // Verifica se utilizador tem pelo menos uma estante de troca
            var minhaEstanteTroca = await _bd.Estante
                .Include(x => x.tipoEstante)
                .Include(x => x.utilizador)
                .Where(x => x.utilizador.utilizadorID == minhaEstante.utilizador.utilizadorID && 
                            x.tipoEstante.descricaoTipoEstante == "Estante Troca")
                .FirstOrDefaultAsync();
            if(minhaEstanteTroca == null) throw new Exception("Utilizador não tem estante de troca, nõo pode existir match");

            // Fazer funcão para procurar match em estante de desejo
            var matchDTO = ProcuraMatchDesejos(minhaEstante, _bd).Result;

            if(matchDTO == null ) throw new Exception("Não existe match");
            
            return matchDTO;
        }else if(minhaEstante.tipoEstante.descricaoTipoEstante == "Estante Troca"){ // Verifica se o tipo de estante é de troca
            // Verifica se utilizador tem pelo menos uma estante de desejo
            var minhaEstanteDesejo = await _bd.Estante
                .Include(x => x.tipoEstante)
                .Include(x => x.utilizador)
                .Where(x => x.utilizador.utilizadorID == minhaEstante.utilizador.utilizadorID && 
                            x.tipoEstante.descricaoTipoEstante == "Estante Desejos")
                .FirstOrDefaultAsync();
            if(minhaEstanteDesejo == null) throw new Exception("Utilizador não tem estante de desejo, não pode existir match");

            // Fazer funcão para procurar match em estante de troca
            var matchDTO = ProcuraMatchTroca(minhaEstante, _bd).Result;
            if(matchDTO == null){
                throw new Exception("Não existe match");
            }


            return matchDTO;

            // return listaEstantes;
        }
        
        return new BadRequestObjectResult(new { message = "Tipo de estante inválido" });  
    
    }

    /// <summary>
    /// Procura match quando leitor adiciona um livro na estante de troca
    /// </summary>
    /// <returns></returns>
    public async Task<MatchDTO> ProcuraMatchTroca(Estante minhaEstante, PageTurnerContext _bd){
        
        MatchDTO matchDTO = new MatchDTO();
        int livroId = minhaEstante.livro.livroId;
        // Procurar o livro nas estantes de desejos de todos os user
        var r = await ProcuraLivroEmEstante(livroId, "Estante Desejos", _bd);
        if (r.Value == null) throw new Exception("Livro não existe em nenhuma estante de desejos"); 
        matchDTO.ListaEstantesComLivroQueEuQuero = r.Value;

        // converter a lista de estantes em lista de utilizadores

        var listUsersQuerLivro = await TransformaEstanteEmUtilizadores(r.Value, _bd);// Temos de fazer isto porque a funcao (ProcuraLivroEmEstante) é ActionResult
        matchDTO.ListUsersTemLivroQueEuQuero = listUsersQuerLivro;


        // Transformar a minha estante de troca numa lista
        // var minhaEstanteList = new List<Estante>();
        var minhaEstanteDesejosList = await _bd.Estante
                                .Include(x => x.livro)
                                .Include(x => x.utilizador)
                                .Where(x => x.tipoEstante.descricaoTipoEstante == "Estante Desejos" && x.utilizador.utilizadorID == minhaEstante.utilizador.utilizadorID)
                                .ToListAsync();



        // minhaEstanteList.Add(minhaEstante);

        //list de todas as minhas estantes de desejos

        // Procurar nas estantes de todos os utilizadores que tem o livro, se têm algum livro que está na minha estante
        var a = await ProcuraLivroEmUtilizadores(listUsersQuerLivro, minhaEstanteDesejosList, "Estante Troca", _bd);
        var listEstantesDosUsersQuerLivroETemLivrosQueMeInteressam = a.Value;
        matchDTO.ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam = listEstantesDosUsersQuerLivroETemLivrosQueMeInteressam;

        return matchDTO ?? throw new Exception("Não existe match");
    }

    /// <summary>
    /// Procura match quando leitor adiciona um livro na estante de desejo
    /// </summary>
    /// <param name="minhaEstante"></param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
     public async Task<MatchDTO> ProcuraMatchDesejos(Estante minhaEstanteDesejos, PageTurnerContext _bd){

        MatchDTO matchDTO = new MatchDTO();
        int livroId = minhaEstanteDesejos.livro.livroId;
        // Procurar o livro que eu desejo nas estantes de troca de todos os leitores
        var r = await ProcuraLivroEmEstante(livroId, "Estante Troca", _bd);
        if (r.Value == null || r.Value.Count <= 0) throw new Exception("Livro não existe em nenhuma estante de troca"); 
        // r -> mostra a lista de estantes que tem o livros que 'eu' quero
        matchDTO.ListaEstantesComLivroQueEuQuero = r.Value;
        // 
        var listUsersTemLivroQueEuQuero =await TransformaEstanteEmUtilizadores(r.Value, _bd);// Temos de fazer isto porque a funcao (ProcuraLivroEmEstante) é ActionResult
        matchDTO.ListUsersTemLivroQueEuQuero = listUsersTemLivroQueEuQuero;

        // #TODO -> Mudar para função das estantes
        // Procura todas as trocas que o leitor tem na sua estante de troca, para que seja possivel 
        // comparar com os livros que outros leitores tenham.
        var listMinhaEstanteTrocas =  await _bd.Estante
            .Include(x => x.tipoEstante)
            .Include(x => x.utilizador)
            .Where(x => x.tipoEstante.descricaoTipoEstante == "Estante Troca" && 
                        x.utilizador.utilizadorID == minhaEstanteDesejos.utilizador.utilizadorID)
            .ToListAsync();

        // Procurar nas estantes de todos os utilizadores que tem o livro, se têm algum livro que está na minha estante
        var a = await ProcuraLivroEmUtilizadores(listUsersTemLivroQueEuQuero, listMinhaEstanteTrocas, "Estante Desejos", _bd);

        var listEstantesDosUsersQuerLivroETemLivrosQueMeInteressam = a.Value;
        matchDTO.ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam = listEstantesDosUsersQuerLivroETemLivrosQueMeInteressam;

        return matchDTO ?? throw new Exception("Não existe match");

    }

    /// <summary>
    /// Faz a procura de um livro em qql estante
    /// </summary>
    /// <param name="livroId"></param>
    /// <param name="estanteId"></param>
    /// <param name="_bd"></param>
    /// <returns>Retorna lista de utilizadores</returns>
    public async Task<ActionResult<List<Estante>>> ProcuraLivroEmEstante(int livroId, string estanteProcura, PageTurnerContext _bd)
    {
        
        //verifica se o livro existe na estante
        //procura livro na estante, se existir devolve ou os utilizadores 
        List<Estante> resp = await _bd.Estante
            .Include(x => x.tipoEstante)
            .Include(x => x.utilizador)
            .Where(x => x.tipoEstante.descricaoTipoEstante == estanteProcura && 
                        x.livro.livroId == livroId)
            .ToListAsync();
        if (resp == null || resp.Count <= 0)
        {
            // erro 
            return null;
        }

        return resp;
    }

    /// <summary>
    /// Transforma uma lista de estantes em uma lista de utilizadores
    /// Verifica se o utilizador tem livros na estante 'oposta', sem isto a troca será sempre incompleta
    /// </summary>
    /// <param name="listaEstantes"></param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    public async Task<List<Utilizador>> TransformaEstanteEmUtilizadores(List<Estante> listaEstantes,  PageTurnerContext _bd)
    {
        //converte a lista de estantes em uma lista de utilizadores
        List<Utilizador> listUser = new List<Utilizador>();

        // Faz uma iteração pela lista de estantes, filtra já os utilizadores que podem ter livros na estante 'oposta'        
        foreach (var item in listaEstantes)
        {
            if(item.tipoEstante.descricaoTipoEstante == "Estante Troca"){ // Devemos mudar isto para enums...
                // Procura se o utilizador tem livros na estante de desejos.
                var estante = await _bd.Estante
                    .Include(x => x.tipoEstante)
                    .Include(x => x.utilizador)
                    .Include(x => x.livro)
                    .Where(x => x.tipoEstante.descricaoTipoEstante == "Estante Desejos" && // Devemos mudar isto para enums...
                                x.utilizador.utilizadorID == item.utilizador.utilizadorID)
                    .FirstOrDefaultAsync();
                if (estante != null) listUser.Add(item.utilizador); 
            }else{
                // Procura se o utilizador tem livros na estante de troca.
                var estante = await _bd.Estante
                    .Include(x => x.tipoEstante)
                    .Include(x => x.utilizador)
                    .Include(x => x.livro)
                    .Where(x => x.tipoEstante.descricaoTipoEstante == "Estante Troca" && // Devemos mudar isto para enums...
                                x.utilizador.utilizadorID == item.utilizador.utilizadorID)
                    .FirstOrDefaultAsync();
                if (estante != null) listUser.Add(item.utilizador);
            }

        }

        return listUser; // retorna a lista de utilizadores, tem de estar assim para podermos usar o Task.

    }


        public async Task<ActionResult<List<Estante>>> ProcuraLivroEmUtilizadores (List<Utilizador> listUsersTemLivroQueEuQuero, 
                                                        List<Estante> listMinhaEstanteTrocas, 
                                                        string estanteProcura, PageTurnerContext _bd)
    {   
        //cria nova lista de utilizadores
        // List<Utilizador> novaLista = new List<Utilizador>();

        //cria nova lista de estantes
        List<Estante> listaEstantes = new List<Estante>();

        //percorre a lista de utilizadores
        foreach (var user in listUsersTemLivroQueEuQuero)
        {
            // verifica na estante de procura (passado por parametros) 
            var estantes = await _bd.Estante
                .Include(x => x.tipoEstante)
                .Include(x => x.utilizador)
                .Include(x => x.livro)
                .Where(x => x.tipoEstante.descricaoTipoEstante == estanteProcura && 
                            x.utilizador.utilizadorID == user.utilizadorID)
                .ToListAsync();

            if (estantes.Count != 0)
            {   // Percorre a lista de estantes de procura 
                foreach (var item in estantes)
                {   // Percorre a lista da minha estante de troca 
                    foreach (var minhaEstante in listMinhaEstanteTrocas){
                        if (item.livro.livroId == minhaEstante.livro.livroId)
                        {
                            listaEstantes.Add(item); // Adiciona estantes à lista
                        }
                    }
                }
            }
        }
        return listaEstantes;
        // return novaLista;
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
            throw new Exception("Estado não existe"); // TODO -> Criar estado caso nao exista na bd, vale a pena?
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
    /// Procura, na BD, troca pelo ID da estante
    /// A ver com pessoal se é necessário
    /// </summary>
    /// <param name="estanteID"></param>
    /// <param name="_bd"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<Troca> ProcTrocaByEstanteID(int estanteID, PageTurnerContext _bd)
    {
        Troca troca = await _bd.Troca
            .Where(x => x.estanteId == estanteID)
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