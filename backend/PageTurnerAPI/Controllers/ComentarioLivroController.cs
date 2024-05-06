using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Services;
using backend.Interfaces;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioLivroController : ControllerBase
    {
        private readonly IPageTurnerContext _context;
        private readonly ComentarioService _comentarioService;

        public ComentarioLivroController(IPageTurnerContext context, ComentarioService comentarioService)
        {
            _context = context;
            _comentarioService = comentarioService;
        }

        /// <summary>
        /// Obtém todos os comentários da DB.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComentarioLivro>>> GetCommentLivro()
        {
            return await _context.ComentarioLivro.ToListAsync();
        }

        /// <summary>
        /// Obtém um comentário específico pelo seu id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ComentarioLivro>> GetComentarioLivro(int id)
        {
            var comentarioLivro = await _context.ComentarioLivro.FindAsync(id);

            if (comentarioLivro == null)
            {
                return NotFound();
            }

            return comentarioLivro;
        }

        /// <summary>
        /// Atualiza um comentário específico.
        /// </summary>
        /// <remarks>
        /// Este método utiliza um DTO (Data Transfer Object) para proteger contra ataques de overposting, garantindo que apenas
        /// os campos permitidos sejam atualizados.
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComentarioLivro(int id, ComentarioLivroDTO comentarioDto)
        {
            if (id != comentarioDto.comentarioId)
            {
                return BadRequest();
            }

            // Encontrar o comentário existente
            var comentarioExistente = await _context.ComentarioLivro.FindAsync(id);
            if (comentarioExistente == null)
            {
                return NotFound();
            }

            // Atualizar as propriedades do comentário existente com os valores do DTO
            comentarioExistente.comentario = comentarioDto.comentario;
            comentarioExistente.dataComentario = comentarioDto.dataComentario;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioLivroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Cria um novo comentário para o livro.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ComentarioLivro>> PostComentarioLivro(ComentarioLivroDTO comentarioDto)
        {
            var estadoInicial = await _context.EstadoComentario
                .FirstOrDefaultAsync(e => e.descricaoEstadoComentario == "Pendente");

            if (estadoInicial == null)
            {
                return BadRequest("Estado padrão 'Pendente' não encontrado.");
            }

            var livro = await _context.Livro
                .Include(l => l.Comentarios)
                .FirstOrDefaultAsync(l => l.livroId == comentarioDto.livroId);

            var user = await _context.Utilizador.FindAsync(comentarioDto.utilizadorId);

            if (user is null)
            {
                return NotFound($"Utilizador com ID {comentarioDto.utilizadorId} não encontrado.");
            }

            if (livro == null)
            {
                return NotFound($"Livro com ID {comentarioDto.livroId} não encontrado.");
            }

            ComentarioLivro novoComentario = new ComentarioLivro
            {
                comentario = comentarioDto.comentario,
                dataComentario = comentarioDto.dataComentario,
                utilizadorId = comentarioDto.utilizadorId,
                livroId = comentarioDto.livroId,
                estadoComentario = estadoInicial
            };

            livro.Comentarios.Add(novoComentario);

            _context.ComentarioLivro.Add(novoComentario);

            // comentario no livro nao está a ser guardado aqui
            await _context.SaveChangesAsync();

            // Inicia a verificação de palavras ofensivas em background #Issue 83
            await novoComentario.VerificarEAtualizarComentario();

            return CreatedAtAction("GetComentarioLivro", new { id = novoComentario.comentarioId }, novoComentario);
        }



        /// <summary>
        /// Elimina um comentário específico do livro.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentarioLivro(int id)
        {
            var comentarioLivro = await _context.ComentarioLivro.FindAsync(id);
            if (comentarioLivro == null)
            {
                return NotFound();
            }

            _context.ComentarioLivro.Remove(comentarioLivro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Obtém todos os comentários associados a um livro específico.
        /// </summary>
        /// <param name="livroId">O identificador do livro para o qual os comentários devem ser recuperados.</param>
        [HttpGet("ByLivro/{livroId}")]
        public async Task<ActionResult<IEnumerable<ComentarioLivro>>> GetComentariosByLivro(int livroId)
        {
            var comentariosDoLivro = await _context.ComentarioLivro
                .Where(cl => cl.livroId == livroId)
                .ToListAsync();

            if (!comentariosDoLivro.Any())
            {
                return NotFound();
            }

            return comentariosDoLivro;
        }


        /// <summary>
        /// Verifica se um comentário do livro existe pelo seu id.
        /// </summary>
        [HttpGet("Exists/{id}")]
        public bool ComentarioLivroExists(int id)
        {
            return _context.ComentarioLivro.Any(e => e.comentarioId == id);
        }

    }
}
