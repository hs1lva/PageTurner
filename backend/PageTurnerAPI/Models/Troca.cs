
using System.ComponentModel.DataAnnotations;
using backend.Controllers;
using backend.Interface;
using backend.SimplifiedModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;
public class Troca
{
    [Key]
    public int trocaId { get; set; }
    public DateTime dataPedidoTroca { get; set; }
    public DateTime? dataAceiteTroca { get; set; }

    // Foreign key for Estante
    public int estanteId { get; set; }
    [ForeignKey("estanteId")]
    public Estante Estante { get; set; }

    // Foreign key for the second Estante (estanteId2)
    public int estanteId2 { get; set; }
    [ForeignKey("estanteId2")]
    public Estante Estante2 { get; set; }

    public EstadoTroca estadoTroca { get; set; }
    private IEmailSender _emailSender;

    #region Construtores
    private readonly PageTurnerContext _bd;

    public Troca() { }

    public Troca(DateTime dataPedidoTroca, int estanteId2, int estanteId, EstadoTroca estadoTroca, IEmailSender emailSender)
    {
        this.dataPedidoTroca = dataPedidoTroca;
        this.estanteId2 = estanteId2;
        this.estanteId = estanteId;
        this.estadoTroca = estadoTroca;
        _emailSender = emailSender;
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

        if (minhaEstante.tipoEstante.descricaoTipoEstante == "Estante Desejos")
        { // Verifica se o tipo de estante é de desejo
            // Verifica se utilizador tem pelo menos uma estante de troca
            var minhaEstanteTroca = await _bd.Estante
                .Include(x => x.tipoEstante)
                .Include(x => x.utilizador)
                .Where(x => x.utilizador.utilizadorID == minhaEstante.utilizador.utilizadorID &&
                            x.tipoEstante.descricaoTipoEstante == "Estante Troca")
                .FirstOrDefaultAsync();
            if (minhaEstanteTroca == null) throw new Exception("Utilizador não tem estante de troca, nõo pode existir match");

            // Fazer funcão para procurar match em estante de desejo
            var matchDTO = ProcuraMatchDesejos(minhaEstante, _bd).Result;

            if (matchDTO == null) throw new Exception("Não existe match");

            return matchDTO;
        }
        else if (minhaEstante.tipoEstante.descricaoTipoEstante == "Estante Troca")
        { // Verifica se o tipo de estante é de troca
            // Verifica se utilizador tem pelo menos uma estante de desejo
            var minhaEstanteDesejo = await _bd.Estante
                .Include(x => x.tipoEstante)
                .Include(x => x.utilizador)
                .Where(x => x.utilizador.utilizadorID == minhaEstante.utilizador.utilizadorID &&
                            x.tipoEstante.descricaoTipoEstante == "Estante Desejos")
                .FirstOrDefaultAsync();
            if (minhaEstanteDesejo == null) throw new Exception("Utilizador não tem estante de desejo, não pode existir match");

            // Fazer funcão para procurar match em estante de troca
            var matchDTO = ProcuraMatchTroca(minhaEstante, _bd).Result;
            if (matchDTO == null)
            {
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
    public async Task<MatchDTO> ProcuraMatchTroca(Estante minhaEstante, PageTurnerContext _bd)
    {

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
    public async Task<MatchDTO> ProcuraMatchDesejos(Estante minhaEstanteDesejos, PageTurnerContext _bd)
    {

        MatchDTO matchDTO = new MatchDTO();
        int livroId = minhaEstanteDesejos.livro.livroId;
        // Procurar o livro que eu desejo nas estantes de troca de todos os leitores
        var r = await ProcuraLivroEmEstante(livroId, "Estante Troca", _bd);
        if (r.Value == null || r.Value.Count <= 0) throw new Exception("Livro não existe em nenhuma estante de troca");
        // r -> mostra a lista de estantes que tem o livros que 'eu' quero
        matchDTO.ListaEstantesComLivroQueEuQuero = r.Value;
        // 
        var listUsersTemLivroQueEuQuero = await TransformaEstanteEmUtilizadores(r.Value, _bd);// Temos de fazer isto porque a funcao (ProcuraLivroEmEstante) é ActionResult
        matchDTO.ListUsersTemLivroQueEuQuero = listUsersTemLivroQueEuQuero;

        // #TODO -> Mudar para função das estantes
        // Procura todas as trocas que o leitor tem na sua estante de troca, para que seja possivel 
        // comparar com os livros que outros leitores tenham.
        var listMinhaEstanteTrocas = await _bd.Estante
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
    public async Task<List<Utilizador>> TransformaEstanteEmUtilizadores(List<Estante> listaEstantes, PageTurnerContext _bd)
    {
        List<Utilizador> listUser = new List<Utilizador>();

        foreach (var item in listaEstantes)
        {
            if (item.tipoEstante.descricaoTipoEstante == "Estante Troca")
            { // Devemos mudar isto para enums...
                // Procura se o utilizador tem livros na estante de desejos.
                var estante = await _bd.Estante
                    .Include(x => x.tipoEstante)
                    .Include(x => x.utilizador)
                    .Include(x => x.livro)
                    .Where(x => x.tipoEstante.descricaoTipoEstante == "Estante Desejos" && // Devemos mudar isto para enums...
                                x.utilizador.utilizadorID == item.utilizador.utilizadorID)
                    .FirstOrDefaultAsync();
                if (estante != null) listUser.Add(item.utilizador);
            }
            else
            {
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


    public async Task<ActionResult<List<Estante>>> ProcuraLivroEmUtilizadores(List<Utilizador> listUsersTemLivroQueEuQuero,
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
                    foreach (var minhaEstante in listMinhaEstanteTrocas)
                    {
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
    public async Task<ActionResult<Troca?>> CriaTroca(int userId, int estanteId, int estanteId2, PageTurnerContext _bd, IEmailSender _emailSender)
    {
        // Procura o estado de troca pendente na BD 
        EstadoTroca estadoTroca = await EstadoTroca.ProcEstadoTroca("Pendente", _bd);

        // Procura as estantes
        var estante1 = await _bd.Estante
                            .Include(e => e.utilizador)
                            .Include(e => e.livro)
                            .FirstOrDefaultAsync(x => x.estanteId == estanteId);

        var estante2 = await _bd.Estante
                            .Include(e => e.utilizador)
                            .Include(e => e.livro)
                            .FirstOrDefaultAsync(x => x.estanteId == estanteId2);

        if (estante1 == null || estante2 == null)
        {
            throw new Exception("Uma das estantes não existe");
        }

        // Verifica se o userId é o utilizador da estante1
        if (estante1.utilizador.utilizadorID != userId)
        {
            // Se não for, troca estante1 com estante2
            var tempEstante = estante1;
            estante1 = estante2;
            estante2 = tempEstante;
        }

        // Cria troca
        var troca = new Troca(
            dataPedidoTroca: DateTime.Now,
            estanteId: estante1.estanteId,
            estanteId2: estante2.estanteId,
            estadoTroca: estadoTroca,
            emailSender: _emailSender
        )
        {
            Estante = estante1,
            Estante2 = estante2
        };

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
    public async Task<ActionResult<Troca?>> TrocaDireta(int userId, int estanteId, PageTurnerContext _bd, IEmailSender _emailSender)
    {
        // User que solicita a troca
        var user = await _bd.Utilizador
            .FirstOrDefaultAsync(x => x.utilizadorID == userId);

        if (user == null)
        {
            throw new Exception("Utilizador que solicita a troca não existe");
        }

        // Procura a estante do livro pedido
        var estanteDoLivroPedido = await _bd.Estante
            .Include(e => e.utilizador)
            .Include(e => e.livro)
            .FirstOrDefaultAsync(x => x.estanteId == estanteId) ?? throw new Exception("Estante do livro pedido não existe");

        // Identificar a estante de quem solicita a troca
        var estanteSolicitante = await _bd.Estante
        .Include(e => e.utilizador)
        .Include(e => e.livro)
        .Where(x => x.utilizador.utilizadorID == userId && x.tipoEstante.descricaoTipoEstante == "troca")
        .FirstOrDefaultAsync();

        if (estanteSolicitante == null)
        {
            throw new Exception("Estante solicitante não existe");
        }

        // Cria a troca
        var resultado = await CriaTroca(userId, estanteSolicitante.estanteId, estanteDoLivroPedido.estanteId, _bd, _emailSender);
        if (resultado.Value != null)
        {
            return resultado.Value;
        }
        else
        {
            throw new Exception("Não foi possível criar a troca");
        }
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

        if (validacao.Value.Item1 == null)
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
            throw new Exception("Estado não existe"); // TODO -> Criar estado caso não exista na bd, vale a pena?
        }
        troca.estadoTroca = estado;

        try
        {
            // Atualizar as estantes dos utilizadores após a troca ser aceita
            var estante1 = await _bd.Estante.FindAsync(troca.estanteId);
            var estante2 = await _bd.Estante.FindAsync(troca.estanteId2);

            if (estante1 != null)
            {
                estante1.livroNaEstante = false;
                _bd.Estante.Update(estante1);
            }

            if (estante2 != null)
            {
                estante2.livroNaEstante = false;
                _bd.Estante.Update(estante2);
            }

            // Atualizar a troca
            _bd.Troca.Update(troca);

            await _bd.SaveChangesAsync();
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
        var troca = await _bd.Troca
            .Include(t => t.estadoTroca)
            .Include(t => t.Estante)
                .ThenInclude(e => e.utilizador)
            .Include(t => t.Estante)
                .ThenInclude(e => e.livro)
            .Include(t => t.Estante2)
                .ThenInclude(e => e.utilizador)
            .Include(t => t.Estante2)
                .ThenInclude(e => e.livro)
            .FirstOrDefaultAsync(t => t.trocaId == id);

        if (troca == null)
        {
            throw new Exception("Troca não encontrada");
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
    private async Task<ActionResult<(Troca?, Utilizador?, Utilizador?)>> ValidaTroca(Troca troca, PageTurnerContext _bd)
    {
        // Obter o utilizador que solicita a troca (está em estante)
        Utilizador? utilizadorSolicitaTroca = await _bd.Utilizador
            .Include(u => u.estadoConta)
            .Where(x => x.utilizadorID == troca.Estante.utilizador.utilizadorID)
            .FirstOrDefaultAsync();

        if (utilizadorSolicitaTroca == null)
        {
            throw new Exception("Utilizador que solicita a troca não existe");
        }

        // Verificar se a estante2 existe e obter seu utilizador
        Estante? estante2 = await _bd.Estante
            .Include(e => e.utilizador)
            .Where(x => x.estanteId == troca.estanteId2)
            .FirstOrDefaultAsync();
        if (estante2 == null)
        {
            throw new Exception("Estante não existe");
        }

        // Obter o utilizador que recebe a troca (está em estante2)
        Utilizador? utilizadorRecebeTroca = estante2.utilizador;
        if (utilizadorRecebeTroca == null)
        {
            throw new Exception("Utilizador que recebe a troca não existe");
        }

        // Verificar se algum dos utilizadores tem a conta suspensa
        if (utilizadorSolicitaTroca.estadoConta.descricaoEstadoConta == "Suspensa" ||
            utilizadorRecebeTroca.estadoConta.descricaoEstadoConta == "Suspensa")
        {
            throw new Exception("Um dos utilizadores não pode fazer trocas");
        }

        // Verificar se o livro da estante existe
        var livro = await _bd.Livro
            .Where(x => x.livroId == troca.Estante.livro.livroId)
            .FirstOrDefaultAsync();
        if (livro == null)
        {
            throw new Exception("Livro não existe");
        }

        

        return (troca, utilizadorSolicitaTroca, utilizadorRecebeTroca);
    }


    public async Task<ActionResult<MatchResultDTO>> ProcuraMatchesParaUtilizador(int utilizadorId, PageTurnerContext _bd)
    {
        MatchResultDTO result = new MatchResultDTO
        {
            Matches = new List<Match>()
        };

        // Recolher todas as estantes de desejos do utilizador
        var estantesDesejos = await _bd.Estante
            .Include(x => x.tipoEstante)
            .Include(x => x.livro)
            .Include(x => x.utilizador)
            .Where(x => x.utilizador.utilizadorID == utilizadorId &&
                        x.tipoEstante.descricaoTipoEstante == "desejos")
            .ToListAsync();

        Console.WriteLine($"Estantes de Desejos: {estantesDesejos.Count}");

        // Recolher todas as estantes de trocas do utilizador
        var estantesTrocas = await _bd.Estante
            .Include(x => x.tipoEstante)
            .Include(x => x.livro)
            .Include(x => x.utilizador)
            .Where(x => x.utilizador.utilizadorID == utilizadorId &&
                        x.tipoEstante.descricaoTipoEstante == "troca" && x.livroNaEstante == true)
            .ToListAsync();

        Console.WriteLine($"Estantes de Trocas: {estantesTrocas.Count}");

        // Procurar matches
        foreach (var estanteDesejo in estantesDesejos)
        {
            // Procurar todas as estantes de troca que tenham o livro desejado
            var estantesTrocaComLivroQueQuero = await _bd.Estante
                .Include(x => x.tipoEstante)
                .Include(x => x.livro)
                .Include(x => x.utilizador)
                .Where(x => x.livro.livroId == estanteDesejo.livro.livroId &&
                            x.tipoEstante.descricaoTipoEstante == "troca" && x.livroNaEstante == true)
                .ToListAsync();

            foreach (var estanteTrocaComLivroQueQuero in estantesTrocaComLivroQueQuero)
            {
                // Procurar todas as estantes de desejos do utilizador que possui o livro de troca
                var estantesDesejosDoOutroUsuario = await _bd.Estante
                    .Include(x => x.tipoEstante)
                    .Include(x => x.livro)
                    .Include(x => x.utilizador)
                    .Where(x => x.utilizador.utilizadorID == estanteTrocaComLivroQueQuero.utilizador.utilizadorID &&
                                x.tipoEstante.descricaoTipoEstante == "desejos" && x.livroNaEstante == true)
                    .ToListAsync();

                // Verificar se o outro utilizador deseja algum dos meus livros de troca
                foreach (var estanteTroca in estantesTrocas)
                {
                    var match = estantesDesejosDoOutroUsuario.FirstOrDefault(x => x.livro.livroId == estanteTroca.livro.livroId);

                    if (match != null)
                    {
                        // Adicionar ao resultado o match encontrado
                        result.Matches.Add(new Match
                        {
                            UserComQueDeiMatch = ConverterParaUtilizadorSimplificado(estanteTrocaComLivroQueQuero.utilizador),
                            QueEuQuero = ConverterParaEstanteSimplificada(estanteTrocaComLivroQueQuero),
                            QueEuTenho = ConverterParaEstanteSimplificada(estanteTroca)
                        });

                        // Exibir informações de debug
                        Console.WriteLine($"Match encontrado: UserComQueDeiMatch={estanteTrocaComLivroQueQuero.utilizador.username}, QueEuQuero={estanteTrocaComLivroQueQuero.livro.tituloLivro}, QueEuTenho={estanteTroca.livro.tituloLivro}");
                    }
                }
            }
        }

        return result;
    }

    private UtilizadorSimplificado ConverterParaUtilizadorSimplificado(Utilizador utilizador)
    {
        return new UtilizadorSimplificado
        {
            UtilizadorID = utilizador.utilizadorID,
            Nome = utilizador.nome,
            Username = utilizador.username
        };
    }

    private EstanteSimplificada ConverterParaEstanteSimplificada(Estante estante)
    {
        return new EstanteSimplificada
        {
            EstanteId = estante.estanteId,
            UltimaAtualizacao = estante.ultimaAtualizacao,
            TipoEstante = estante.tipoEstante.descricaoTipoEstante,
            Utilizador = new UtilizadorSimplificado
            {
                UtilizadorID = estante.utilizador.utilizadorID,
                Nome = estante.utilizador.nome,
                Username = estante.utilizador.username
            },
            Livro = new LivroSimplificado
            {
                LivroId = estante.livro.livroId,
                TituloLivro = estante.livro.tituloLivro,
                CapaSmall = estante.livro.capaSmall
            },
            LivroNaEstante = estante.livroNaEstante
        };
    }
}