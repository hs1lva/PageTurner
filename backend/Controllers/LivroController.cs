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
    public class LivroController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public LivroController(PageTurnerContext context)
        {
            _context = context;
        }

        #region Métodos GET
        // GET: api/Livro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivro()
        {
            return await _context.Livro.ToListAsync();
        }

        // GET: api/Livro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Livro>> GetLivro(int id)
        {
            var livro = await _context.Livro.FindAsync(id);

            if (livro == null)
            {
                return NotFound();
            }

            return livro;
        }

        /// <summary>
        /// Método para pesquisar um livro pelo título
        /// </summary>
        /// <param name="termo"></param>
        /// <returns></returns>
        // GET: api/Livro/Pesquisar/{termo}
        [HttpGet("Pesquisar/{termo}")]
        public async Task<ActionResult<IEnumerable<Livro>>> PesquisarLivro(string termo)
        {
            var livro = await _context.Livro
                .Where(l => l.tituloLivro.ToLower() == termo.ToLower())
                .ToListAsync();

            if (livro == null || livro.Count == 0)
            {
                livro = await _context.Livro
                    .Where(l => l.tituloLivro.ToLower().Contains(termo.ToLower()))
                    .ToListAsync();
            }

            if (livro == null || livro.Count == 0)
            {
                return NotFound();
            }

            if (livro.Count == 1)
                {
                    return Ok(livro.First());
                }

            return livro;
        }

        // GET: api/Livro/GetPerfilLivro/{id}
        [HttpGet("GetPerfilLivro/{id}")]
        public async Task<ActionResult<Livro>> PerfilLivro(int id)
        {
            try
            {
                var livro = await _context.Livro
                                            .Include(l => l.autorLivro)
                                            .Include(l => l.generoLivro)
                                            .Include(l => l.Avaliacoes)
                                            .FirstOrDefaultAsync(l => l.livroId == id);

                if (livro == null)
                {
                    return NotFound();
                }

                // var comentarios = await _context.ComentarioLivro
                //                                 .Where(c => c.LivroId == id)
                //                                 .ToListAsync();

                // if (comentarios.Any())
                // {
                //     livro.Comentarios = comentarios;
                // }

                return livro;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter detalhes do livro: {ex.Message}");
            }
        }

        #endregion

        #region Métodos PUT
        // PUT: api/Livro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivro(int id, Livro livro)
        {
            if (id != livro.livroId)
            {
                return BadRequest();
            }

            _context.Entry(livro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(id))
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

        #endregion

        
        #region Métodos POST
        // POST: api/Livro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Livro>> PostLivro(Livro livro)
        {
            _context.Livro.Add(livro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLivro", new { id = livro.livroId }, livro);
        }
        #endregion

        #region Métodos DELETE
        // DELETE: api/Livro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivro(int id)
        {
            var livro = await _context.Livro.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }

            _context.Livro.Remove(livro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
        private bool LivroExists(int id)
        {
            return _context.Livro.Any(e => e.livroId == id);
        }
        
    }
}
