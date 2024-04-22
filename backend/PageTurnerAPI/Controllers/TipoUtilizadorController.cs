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
    public class TipoUtilizadorController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public TipoUtilizadorController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/TipoUtilizador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoUtilizador>>> GetTipoUtilizador()
        {
            return await _context.TipoUtilizador.ToListAsync();
        }

        // GET: api/TipoUtilizador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUtilizador>> GetTipoUtilizador(int id)
        {
            var tipoUtilizador = await _context.TipoUtilizador.FindAsync(id);

            if (tipoUtilizador == null)
            {
                return NotFound();
            }

            return tipoUtilizador;
        }

        // PUT: api/TipoUtilizador/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoUtilizador(int id, TipoUtilizador tipoUtilizador)
        {
            if (id != tipoUtilizador.tipoUtilId)
            {
                return BadRequest();
            }

            _context.Entry(tipoUtilizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoUtilizadorExists(id))
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

        // POST: api/TipoUtilizador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoUtilizador>> PostTipoUtilizador(TipoUtilizador tipoUtilizador)
        {
            _context.TipoUtilizador.Add(tipoUtilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoUtilizador", new { id = tipoUtilizador.tipoUtilId }, tipoUtilizador);
        }

        // DELETE: api/TipoUtilizador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoUtilizador(int id)
        {
            var tipoUtilizador = await _context.TipoUtilizador.FindAsync(id);
            if (tipoUtilizador == null)
            {
                return NotFound();
            }

            _context.TipoUtilizador.Remove(tipoUtilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoUtilizadorExists(int id)
        {
            return _context.TipoUtilizador.Any(e => e.tipoUtilId == id);
        }
    }
}
