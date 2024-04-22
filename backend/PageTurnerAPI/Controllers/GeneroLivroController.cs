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
    public class GeneroLivroController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public GeneroLivroController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/GeneroLivro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneroLivro>>> GetGeneroLivro()
        {
            return await _context.GeneroLivro.ToListAsync();
        }

        // GET: api/GeneroLivro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneroLivro>> GetGeneroLivro(int id)
        {
            var generoLivro = await _context.GeneroLivro.FindAsync(id);

            if (generoLivro == null)
            {
                return NotFound();
            }

            return generoLivro;
        }

        // PUT: api/GeneroLivro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneroLivro(int id, GeneroLivro generoLivro)
        {
            if (id != generoLivro.generoId)
            {
                return BadRequest();
            }

            _context.Entry(generoLivro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneroLivroExists(id))
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

        // POST: api/GeneroLivro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GeneroLivro>> PostGeneroLivro(GeneroLivro generoLivro)
        {
            _context.GeneroLivro.Add(generoLivro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeneroLivro", new { id = generoLivro.generoId }, generoLivro);
        }

        // DELETE: api/GeneroLivro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeneroLivro(int id)
        {
            var generoLivro = await _context.GeneroLivro.FindAsync(id);
            if (generoLivro == null)
            {
                return NotFound();
            }

            _context.GeneroLivro.Remove(generoLivro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneroLivroExists(int id)
        {
            return _context.GeneroLivro.Any(e => e.generoId == id);
        }
    }
}
