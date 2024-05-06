using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    /// <summary>
    /// Representa um comentário feito em um livro dentro da plataforma.
    /// </summary>
    public class ComentarioLivro
    {
        [Key]
        public int comentarioId { get; set; }

        [Required(ErrorMessage = "O conteúdo do comentário é obrigatório.")]
        public string comentario { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime dataComentario { get; set; }

        /// <summary>
        /// Estado atual do comentário (por exemplo, pendente, ativo, removido).
        /// </summary>
        public EstadoComentario estadoComentario { get; set; }

        /// <summary>
        /// Lista de conteúdos ofensivos associados a este comentário, se houver.
        /// </summary>
        public ICollection<ComentarioLivroConteudoOfensivo> comentarioConteudoOfensivo { get; set; }

        /// <summary>
        /// Identificador do utilizador que submeteu o comentário.
        /// </summary>
        [Required]
        public int utilizadorId { get; set; }

        /// <summary>
        /// Identificador do livro ao qual o comentário está associado.
        /// </summary>
        [Required]
        public int livroId { get; set; }

        private readonly PageTurnerContext _context;

        public ComentarioLivro() { }
        public ComentarioLivro(PageTurnerContext pageTurnerContext)
        {
            this._context = pageTurnerContext;
        }

        /// <summary>
        /// Verifica o conteúdo de um comentário para identificar a presença de conteúdo ofensivo e atualiza o estado do comentário conforme necessário.
        /// Também gere as relações entre comentários e conteúdos ofensivos identificados.
        /// </summary>
        /// <param name="comentarioId">ID do comentário a ser verificado e atualizado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. #Issue 83</returns>
        /// </remarks>
        public async Task VerificarEAtualizarComentario()
        {
            try
            {

                // Obter estados necessários para atualização
                var estadoAtivo = await ObterEstadoComentario("Ativo");
                var estadoEliminado = await ObterEstadoComentario("Removido");

                // Verificar conteúdo ofensivo e atualizar estado do comentário
                await VerificarEProcessarConteudoOfensivo(estadoAtivo, estadoEliminado);

                // Verificar se o usuário que fez o comentário deve ser banido
                await VerificarEBanirUser(utilizadorId);

                // Salvar as alterações na DB
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Verifica se o usuário deve ser banido após três comentários ofensivos e, em caso afirmativo, executa o banimento.
        /// </summary>
        /// <param name="usuarioId">ID do usuário a ser verificado e potencialmente banido.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        private async Task VerificarEBanirUser(int usuarioId)
        {
            // Verifica quantos comentários com conteúdo ofensivo o usuário tem
            var comentariosOfensivosRemovidosCount = await _context.ComentarioLivroConteudoOfensivo
                .Where(c => c.comentarioLivro.utilizadorId == usuarioId)
                .CountAsync();

            // Se o usuário tiver três ou mais comentários ofensivos, banir o usuário
            if (comentariosOfensivosRemovidosCount >= 3)
            {
                var user = await _context.Utilizador.FindAsync(usuarioId);
                if (user != null)
                {
                    await user.BanirUtilizador();
                }
            }
        }

        /// <summary>
        /// Obtém um comentário específico pelo ID.
        /// </summary>
        /// <param name="comentarioId">ID do comentário a ser procurado.</param>
        /// <returns>Um objeto `ComentarioLivro` que representa o comentário encontrado, ou `null` se não for encontrado.</returns>
        /// <exception cref="Exception">Ocorre quando o comentário não é encontrado.</exception>

        public async Task<ComentarioLivro> BuscarComentario(int comentarioId)
        {
            var comentario = await _context.ComentarioLivro
                .Include(c => c.estadoComentario)
                .FirstOrDefaultAsync(c => c.comentarioId == comentarioId);
            if (comentario == null)
            {
                throw new Exception("Comentário não encontrado.");
            }
            return comentario;
        }


        /// <summary>
        /// Obtém um estado de comentário pelo nome da descrição.
        /// </summary>
        /// <param name="descricao">A descrição do estado do comentário a ser obtido.</param>
        /// <returns>O estado do comentário correspondente à descrição fornecida.</returns>
        /// <remarks>
        /// Este método é usado internamente para obter os estados de comentário necessários
        /// para a atualização do estado do comentário baseado na presença de conteúdo ofensivo.
        /// #Issue 83
        /// </remarks>
        public async Task<EstadoComentario> ObterEstadoComentario(string descricao)
        {
            return await _context.EstadoComentario
                .FirstOrDefaultAsync(e => e.descricaoEstadoComentario == descricao);
        }


        /// <summary>
        /// Verifica o conteúdo de um comentário para identificar a presença de conteúdo ofensivo e atualiza o estado do comentário conforme necessário.
        /// </summary>
        /// <param name="comentario">O objeto `ComentarioLivro` que contém o comentário a ser verificado.</param>
        /// <param name="estadoAtivo">O objeto `EstadoComentario` que representa o estado "Ativo".</param>
        /// <param name="estadoEliminado">O objeto `EstadoComentario` que representa o estado "Removido".</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>

        public async Task VerificarEProcessarConteudoOfensivo(EstadoComentario estadoAtivo, EstadoComentario estadoEliminado)
        {
            var conteudosOfensivos = await IdentificarConteudoOfensivo(comentario);

            if (conteudosOfensivos.Any())
            {
                estadoComentario = estadoEliminado;
                await AdicionarRelacoesConteudoOfensivo(conteudosOfensivos);
            }
            else
            {
                estadoComentario = estadoAtivo;
            }
        }


        /// <summary>
        /// Identifica palavras ou expressões ofensivas dentro de um comentário e retorna os IDs das correspondências encontradas.
        /// </summary>
        /// <param name="comentario">O texto do comentário a ser avaliado.</param>
        /// <returns>Uma lista de IDs representando os conteúdos ofensivos encontrados, ou uma lista vazia caso não haja correspondência.</returns>

        public virtual async Task<List<int>> IdentificarConteudoOfensivo(string comentario)
        {
            var conteudosOfensivos = await _context.ConteudoOfensivo.ToListAsync();
            var palavrasOfensivas = conteudosOfensivos.Select(co => co.especificacaoConteudoOfensivo.ToLower()).ToList();

            var idsOfensivos = new List<int>();
            foreach (var conteudo in conteudosOfensivos)
            {
                if (comentario.ToLower().Contains(conteudo.especificacaoConteudoOfensivo.ToLower()))
                {
                    idsOfensivos.Add(conteudo.conteudoOfensivoId);
                }
            }

            return idsOfensivos;
        }

        /// <summary>
        ///  Cria associações entre um comentário e conteúdos ofensivos identificados.
        /// </summary>
        /// <param name="comentarioId">O ID do comentário.</param>
        /// <param name="conteudosOfensivos">Uma lista de IDs representando os conteúdos ofensivos encontrados no comentário.</param>

        private async Task AdicionarRelacoesConteudoOfensivo(IEnumerable<int> conteudosOfensivos)
        {
            foreach (var conteudoOfensivoId in conteudosOfensivos)
            {
                var existe = await _context.ComentarioLivroConteudoOfensivo.AnyAsync(co => co.comentarioId == comentarioId && co.conteudoOfensivoId == conteudoOfensivoId);
                if (!existe)
                {
                    _context.ComentarioLivroConteudoOfensivo.Add(new ComentarioLivroConteudoOfensivo
                    {
                        comentarioId = comentarioId,
                        conteudoOfensivoId = conteudoOfensivoId
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

    }

}