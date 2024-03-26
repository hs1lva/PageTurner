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
    public class TrocaController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public TrocaController(PageTurnerContext context)
        {
            _context = context;
        }

        #region CRUD
        // GET: api/Troca
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Troca>>> GetTroca()
        {
            return await _context.Troca.ToListAsync();
        }

        // GET: api/Troca/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Troca>> GetTroca(int id)
        {
            var troca = await _context.Troca.FindAsync(id);

            if (troca == null)
            {
                return NotFound();
            }

            return troca;
        }

        // PUT: api/Troca/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTroca(int id, Troca troca)
        {
            if (id != troca.trocaId)
            {
                return BadRequest();
            }

            _context.Entry(troca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrocaExists(id))
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

        // POST: api/Troca
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Troca>> PostTroca()
        {
            #region Dados para teste
            DateTime dataPedido = DateTime.Now;
            Estante estanteId2 = await _context.Estante
                            .Where(x => x.estanteId == 12)
                            .FirstOrDefaultAsync();
            var estanteId = 13;
            EstadoTroca estadoTroca = await _context.EstadoTroca
                                        .Where(x => x.estadoTrocaId == 1)
                                        .FirstOrDefaultAsync();
            Troca troca = new Troca(dataPedido, estanteId2, estanteId, estadoTroca);
            #endregion

            _context.Troca.Add(troca);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTroca", new { id = troca.trocaId }, troca);
        }

        // DELETE: api/Troca/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTroca(int id)
        {
            var troca = await _context.Troca.FindAsync(id);
            if (troca == null)
            {
                return NotFound();
            }

            _context.Troca.Remove(troca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrocaExists(int id)
        {
            return _context.Troca.Any(e => e.trocaId == id);
        }
        #endregion


        //funcionalidade de solicitação de troca de um livro a partir da estante de trocas no perfil de um leitor
        [HttpPost("solicita-troca")]
        public Task<IActionResult> PostAdicionaTroca()
        {
            throw new NotImplementedException();
        }
    }
}
