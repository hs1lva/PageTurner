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
    public class AvaliacaoLivroController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public AvaliacaoLivroController(PageTurnerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as avaliações de livros.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvaliacaoLivro>>> GetAvaliacaoLivro()
        {
            return await _context.AvaliacaoLivro.ToListAsync();
        }

        /// <summary>
        /// Obtém uma avaliação de livro específica pelo seu id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AvaliacaoLivro>> GetAvaliacaoLivro(int id)
        {
            var avaliacaoLivro = await _context.AvaliacaoLivro.FindAsync(id);

            if (avaliacaoLivro == null)
            {
                return NotFound();
            }

            return avaliacaoLivro;
        }

        // <summary>
        // Atualiza uma avaliação específica.
        // </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvaliacaoLivro(int id, AvaliacaoLivro avaliacaoLivroDTO)
        {

            // Busca a avaliação existente pelo ID
            var avaliacaoExistente = await _context.AvaliacaoLivro.FindAsync(id);
            if (avaliacaoExistente == null)
            {
                return NotFound(new { Message = "Avaliação não encontrada." });
            }

            // Verifica se a avaliação corresponde ao livro correto
            if (avaliacaoExistente.LivroId != avaliacaoLivroDTO.LivroId)
            {
                return BadRequest(new { Message = "A avaliação não corresponde ao livro fornecido." });
            }

            // Verifica se o utilizador existe
            var utilizadorExistente = await _context.Utilizador.FindAsync(avaliacaoLivroDTO.UtilizadorId);
            if (utilizadorExistente == null)
            {
                return NotFound(new { Message = "Utilizador não encontrado." });
            }

            // Atualiza a avaliação existente com os dados do DTO 
            _context.Entry(avaliacaoExistente).CurrentValues.SetValues(avaliacaoLivroDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AvaliacaoLivroExists(id))
                {
                    return NotFound(new { Message = "Avaliação não encontrada ao tentar salvar as alterações." });
                }
                else
                {
                    return StatusCode(500, new { Message = "Ocorreu um erro ao salvar as alterações." });
                }
            }

            return NoContent(); 
        }

        // <summary>
        // Insere uma nova avaliação.
        // </summary>
        [HttpPost]
        public async Task<ActionResult<AvaliacaoLivro>> PostAvaliacaoLivro(AvaliacaoLivro avaliacaoLivro)
        {
            // Validações
            
            // Verifica se o user existe
            var user = await _context.Utilizador.FindAsync(avaliacaoLivro.UtilizadorId);

            if (user is null)
            {
                return NotFound($"Utilizador com ID {avaliacaoLivro.UtilizadorId} não encontrado.");
            }
            
            // Validar se o user já fez uma avaliação para este livro
            bool avaliacaoExistente = await _context.AvaliacaoLivro.AnyAsync(a => a.LivroId == avaliacaoLivro.LivroId && a.UtilizadorId == avaliacaoLivro.UtilizadorId);
            if (avaliacaoExistente)
            {
                return BadRequest(new { Message = "O utilizador já fez uma avaliação para este livro." });
            }

            // Verificar se o livro existe
            Livro livro = await _context.Livro.FindAsync(avaliacaoLivro.LivroId);
            if (livro == null)
            {
                return NotFound(new { Message = "Livro não encontrado" });
            }
            
             // Verifica se ModelState é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
    
            _context.AvaliacaoLivro.Add(avaliacaoLivro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAvaliacaoLivro", new { id = avaliacaoLivro.AvaliacaoId }, avaliacaoLivro);
        }

        // <summary>
        // Elimina uma avaliação específica.
        // </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvaliacaoLivro(int id)
        {
            var avaliacaoLivro = await _context.AvaliacaoLivro.FindAsync(id);
            if (avaliacaoLivro == null)
            {
                return NotFound();
            }

            _context.AvaliacaoLivro.Remove(avaliacaoLivro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AvaliacaoLivroExists(int id)
        {
            return _context.AvaliacaoLivro.Any(e => e.AvaliacaoId == id);
        }
    }
}
