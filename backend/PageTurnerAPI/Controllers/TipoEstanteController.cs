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
    public class TipoEstanteController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public TipoEstanteController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/TipoEstante
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoEstante>>> GetTipoEstante()
        {
            return await _context.TipoEstante.ToListAsync();
        }

        // GET: api/TipoEstante/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoEstante>> GetTipoEstante(int id)
        {
            var tipoEstante = await _context.TipoEstante.FindAsync(id);

            if (tipoEstante == null)
            {
                return NotFound();
            }

            return tipoEstante;
        }

        // PUT: api/TipoEstante/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoEstante(int id, TipoEstante tipoEstante)
        {
            if (id != tipoEstante.tipoEstanteId)
            {
                return BadRequest();
            }

            _context.Entry(tipoEstante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoEstanteExists(id))
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

        // POST: api/TipoEstante
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoEstante>> PostTipoEstante(TipoEstante tipoEstante)
        {
            _context.TipoEstante.Add(tipoEstante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoEstante", new { id = tipoEstante.tipoEstanteId }, tipoEstante);
        }

        // DELETE: api/TipoEstante/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoEstante(int id)
        {
            var tipoEstante = await _context.TipoEstante.FindAsync(id);
            if (tipoEstante == null)
            {
                return NotFound();
            }

            _context.TipoEstante.Remove(tipoEstante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoEstanteExists(int id)
        {
            return _context.TipoEstante.Any(e => e.tipoEstanteId == id);
        }
    }
}
