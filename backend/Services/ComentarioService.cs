using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ComentarioService
    {
        private  readonly PageTurnerContext _context;
        
        public ComentarioService(PageTurnerContext context)
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
        public async Task<List<int>> IdentificarConteudoOfensivoAsync(string comentario)
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
    }
}