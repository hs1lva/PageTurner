using Microsoft.EntityFrameworkCore;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    public class ComentarioService
    {
        private  readonly IPageTurnerContext _context;
        
        public ComentarioService(IPageTurnerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Identifica conteúdo ofensivo num comentário e retorna os IDs do conteúdo ofensivo encontrado.
        /// </summary>
        /// <param name="comentario">O texto do comentário a ser analisado.</param>
        /// <returns>
        /// Uma lista de IDs de conteúdo ofensivo que foram identificados no comentário.
        /// Se nenhum conteúdo ofensivo for encontrado, retorna uma lista vazia.
        /// </returns>
        /// <remarks>
        /// O método compara o comentário fornecido com a lista de palavras ofensivas pré-definidas
        /// na BD. A comparação é feita de maneira insensível a maiúsculas e minúsculas.
        /// #Issue 83
        /// </remarks>
        public virtual async Task<List<int>> IdentificarConteudoOfensivoAsync(string comentario)
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

         public async Task VerificarEProcessarConteudoOfensivoAsync(ComentarioLivro comentario, EstadoComentario estadoAtivo, EstadoComentario estadoEliminado)
        {
            var conteudosOfensivos = await IdentificarConteudoOfensivoAsync(comentario.comentario);

            if (conteudosOfensivos.Any())
            {
                comentario.estadoComentario = estadoEliminado;
                await AdicionarRelacoesConteudoOfensivo(comentario.comentarioId, conteudosOfensivos);
            }
            else
            {
                comentario.estadoComentario = estadoAtivo;
            }
        }

        private async Task AdicionarRelacoesConteudoOfensivo(int comentarioId, IEnumerable<int> conteudosOfensivos)
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