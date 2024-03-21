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
    public class AutorLivroController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public AutorLivroController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/AutorLivro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorLivro>>> GetAutorLivro()
        {
            return await _context.AutorLivro.ToListAsync();
        }

        // GET: api/AutorLivro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AutorLivro>> GetAutorLivro(int id)
        {
            var autorLivro = await _context.AutorLivro.FindAsync(id);

            if (autorLivro == null)
            {
                return NotFound();
            }

            return autorLivro;
        }

        // PUT: api/AutorLivro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutorLivro(int id, AutorLivro autorLivro)
        {
            if (id != autorLivro.autorLivroId)
            {
                return BadRequest();
            }

            _context.Entry(autorLivro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorLivroExists(id))
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

        // POST: api/AutorLivro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AutorLivro>> PostAutorLivro(AutorLivro autorLivro)
        {
            _context.AutorLivro.Add(autorLivro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAutorLivro", new { id = autorLivro.autorLivroId }, autorLivro);
        }

        // DELETE: api/AutorLivro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutorLivro(int id)
        {
            var autorLivro = await _context.AutorLivro.FindAsync(id);
            if (autorLivro == null)
            {
                return NotFound();
            }

            _context.AutorLivro.Remove(autorLivro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AutorLivroExists(int id)
        {
            return _context.AutorLivro.Any(e => e.autorLivroId == id);
        }
    }
}
