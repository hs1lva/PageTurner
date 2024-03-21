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
    public class ConteudoOfensivoController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public ConteudoOfensivoController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/ConteudoOfensivo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConteudoOfensivo>>> GetConteudoOfensivo()
        {
            return await _context.ConteudoOfensivo.ToListAsync();
        }

        // GET: api/ConteudoOfensivo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConteudoOfensivo>> GetConteudoOfensivo(int id)
        {
            var conteudoOfensivo = await _context.ConteudoOfensivo.FindAsync(id);

            if (conteudoOfensivo == null)
            {
                return NotFound();
            }

            return conteudoOfensivo;
        }

        // PUT: api/ConteudoOfensivo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConteudoOfensivo(int id, ConteudoOfensivo conteudoOfensivo)
        {
            if (id != conteudoOfensivo.conteudoOfensivoId)
            {
                return BadRequest();
            }

            _context.Entry(conteudoOfensivo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConteudoOfensivoExists(id))
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

        // POST: api/ConteudoOfensivo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConteudoOfensivo>> PostConteudoOfensivo(ConteudoOfensivo conteudoOfensivo)
        {
            _context.ConteudoOfensivo.Add(conteudoOfensivo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConteudoOfensivo", new { id = conteudoOfensivo.conteudoOfensivoId }, conteudoOfensivo);
        }

        // DELETE: api/ConteudoOfensivo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConteudoOfensivo(int id)
        {
            var conteudoOfensivo = await _context.ConteudoOfensivo.FindAsync(id);
            if (conteudoOfensivo == null)
            {
                return NotFound();
            }

            _context.ConteudoOfensivo.Remove(conteudoOfensivo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConteudoOfensivoExists(int id)
        {
            return _context.ConteudoOfensivo.Any(e => e.conteudoOfensivoId == id);
        }
    }
}
