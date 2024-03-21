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
    public class EstanteController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public EstanteController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/Estante
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estante>>> GetEstante()
        {
            return await _context.Estante.ToListAsync();
        }

        // GET: api/Estante/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estante>> GetEstante(int id)
        {
            var estante = await _context.Estante.FindAsync(id);

            if (estante == null)
            {
                return NotFound();
            }

            return estante;
        }

        // PUT: api/Estante/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstante(int id, Estante estante)
        {
            if (id != estante.estanteId)
            {
                return BadRequest();
            }

            _context.Entry(estante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstanteExists(id))
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

        // POST: api/Estante
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estante>> PostEstante(Estante estante)
        {
            _context.Estante.Add(estante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstante", new { id = estante.estanteId }, estante);
        }

        // DELETE: api/Estante/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstante(int id)
        {
            var estante = await _context.Estante.FindAsync(id);
            if (estante == null)
            {
                return NotFound();
            }

            _context.Estante.Remove(estante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstanteExists(int id)
        {
            return _context.Estante.Any(e => e.estanteId == id);
        }
    }
}
