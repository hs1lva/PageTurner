using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioLivroController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public ComentarioLivroController(PageTurnerContext context)
        {
            _context = context;
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
            if (id != comentarioDto.ComentarioId)
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
            comentarioExistente.Comentario = comentarioDto.Comentario;
            comentarioExistente.DataComentario = comentarioDto.DataComentario;

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

            ComentarioLivro novoComentario = new ComentarioLivro
            {
                Comentario = comentarioDto.Comentario,
                DataComentario = comentarioDto.DataComentario,
                UtilizadorId = comentarioDto.UtilizadorId,
                LivroId = comentarioDto.LivroId,
                EstadoComentario = estadoInicial 
            };

            _context.ComentarioLivro.Add(novoComentario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComentarioLivro", new { id = novoComentario.ComentarioId }, novoComentario);
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
                .Where(cl => cl.LivroId == livroId)
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
        private bool ComentarioLivroExists(int id)
        {
            return _context.ComentarioLivro.Any(e => e.ComentarioId == id);
        }
    }
}
