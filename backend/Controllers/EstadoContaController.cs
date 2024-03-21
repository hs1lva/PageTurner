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
    public class EstadoContaController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public EstadoContaController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/EstadoConta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoConta>>> GetEstadoConta()
        {
            return await _context.EstadoConta.ToListAsync();
        }

        // GET: api/EstadoConta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoConta>> GetEstadoConta(int id)
        {
            var estadoConta = await _context.EstadoConta.FindAsync(id);

            if (estadoConta == null)
            {
                return NotFound();
            }

            return estadoConta;
        }

        // PUT: api/EstadoConta/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoConta(int id, EstadoConta estadoConta)
        {
            if (id != estadoConta.estadoContaId)
            {
                return BadRequest();
            }

            _context.Entry(estadoConta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoContaExists(id))
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

        // POST: api/EstadoConta
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoConta>> PostEstadoConta(EstadoConta estadoConta)
        {
            _context.EstadoConta.Add(estadoConta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoConta", new { id = estadoConta.estadoContaId }, estadoConta);
        }

        // DELETE: api/EstadoConta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoConta(int id)
        {
            var estadoConta = await _context.EstadoConta.FindAsync(id);
            if (estadoConta == null)
            {
                return NotFound();
            }

            _context.EstadoConta.Remove(estadoConta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoContaExists(int id)
        {
            return _context.EstadoConta.Any(e => e.estadoContaId == id);
        }
    }
}
