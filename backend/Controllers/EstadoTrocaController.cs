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
    public class EstadoTrocaController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public EstadoTrocaController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/EstadoTroca
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoTroca>>> GetEstadoTroca()
        {
            return await _context.EstadoTroca.ToListAsync();
        }

        // GET: api/EstadoTroca/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoTroca>> GetEstadoTroca(int id)
        {
            var estadoTroca = await _context.EstadoTroca.FindAsync(id);

            if (estadoTroca == null)
            {
                return NotFound();
            }

            return estadoTroca;
        }

        // PUT: api/EstadoTroca/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoTroca(int id, EstadoTroca estadoTroca)
        {
            if (id != estadoTroca.estadoTrocaId)
            {
                return BadRequest();
            }

            _context.Entry(estadoTroca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoTrocaExists(id))
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

        // POST: api/EstadoTroca
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoTroca>> PostEstadoTroca(EstadoTroca estadoTroca)
        {
            _context.EstadoTroca.Add(estadoTroca);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoTroca", new { id = estadoTroca.estadoTrocaId }, estadoTroca);
        }

        // DELETE: api/EstadoTroca/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoTroca(int id)
        {
            var estadoTroca = await _context.EstadoTroca.FindAsync(id);
            if (estadoTroca == null)
            {
                return NotFound();
            }

            _context.EstadoTroca.Remove(estadoTroca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoTrocaExists(int id)
        {
            return _context.EstadoTroca.Any(e => e.estadoTrocaId == id);
        }
    }
}
