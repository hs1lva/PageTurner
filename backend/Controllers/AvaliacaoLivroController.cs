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

        // GET: api/AvaliacaoLivro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvaliacaoLivro>>> GetAvaliacaoLivro()
        {
            return await _context.AvaliacaoLivro.ToListAsync();
        }

        // GET: api/AvaliacaoLivro/5
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

        // PUT: api/AvaliacaoLivro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvaliacaoLivro(int id, AvaliacaoLivro avaliacaoLivro)
        {
            if (id != avaliacaoLivro.avaliacaoId)
            {
                return BadRequest();
            }

            _context.Entry(avaliacaoLivro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvaliacaoLivroExists(id))
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

        // POST: api/AvaliacaoLivro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AvaliacaoLivro>> PostAvaliacaoLivro(AvaliacaoLivro avaliacaoLivro)
        {
            _context.AvaliacaoLivro.Add(avaliacaoLivro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAvaliacaoLivro", new { id = avaliacaoLivro.avaliacaoId }, avaliacaoLivro);
        }

        // DELETE: api/AvaliacaoLivro/5
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
            return _context.AvaliacaoLivro.Any(e => e.avaliacaoId == id);
        }
    }
}
