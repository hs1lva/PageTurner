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
    public class EstadoComentarioController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public EstadoComentarioController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/EstadoComentario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoComentario>>> GetEstadoComentario()
        {
            return await _context.EstadoComentario.ToListAsync();
        }

        // GET: api/EstadoComentario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoComentario>> GetEstadoComentario(int id)
        {
            var estadoComentario = await _context.EstadoComentario.FindAsync(id);

            if (estadoComentario == null)
            {
                return NotFound();
            }

            return estadoComentario;
        }

        // PUT: api/EstadoComentario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoComentario(int id, EstadoComentario estadoComentario)
        {
            if (id != estadoComentario.estadoComentarioId)
            {
                return BadRequest();
            }

            _context.Entry(estadoComentario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoComentarioExists(id))
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

        // POST: api/EstadoComentario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoComentario>> PostEstadoComentario(EstadoComentario estadoComentario)
        {
            _context.EstadoComentario.Add(estadoComentario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoComentario", new { id = estadoComentario.estadoComentarioId }, estadoComentario);
        }

        // DELETE: api/EstadoComentario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoComentario(int id)
        {
            var estadoComentario = await _context.EstadoComentario.FindAsync(id);
            if (estadoComentario == null)
            {
                return NotFound();
            }

            _context.EstadoComentario.Remove(estadoComentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoComentarioExists(int id)
        {
            return _context.EstadoComentario.Any(e => e.estadoComentarioId == id);
        }
    }
}
