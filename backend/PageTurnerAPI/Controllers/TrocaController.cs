using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Interface;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrocaController : ControllerBase
    {
        private readonly PageTurnerContext _context;
        private readonly IEmailSender _emailSender;

        public TrocaController(PageTurnerContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        #region CRUD
        // GET: api/Troca
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Troca>>> GetTroca()
        {
            return await _context.Troca
                            .Include(x => x.estadoTroca)
                            .ToListAsync();
        }

        // GET: api/Troca/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Troca>> GetTroca(int id)
        {
            var troca = await _context.Troca
                .Include(t => t.Estante)
                    .ThenInclude(e => e.tipoEstante)
                .Include(t => t.Estante)
                    .ThenInclude(e => e.utilizador)
                .Include(t => t.Estante)
                    .ThenInclude(e => e.livro)
                .Include(t => t.Estante2)
                    .ThenInclude(e => e.tipoEstante)
                .Include(t => t.Estante2)
                    .ThenInclude(e => e.utilizador)
                .Include(t => t.Estante2)
                    .ThenInclude(e => e.livro)
                .Include(t => t.estadoTroca)
                .FirstOrDefaultAsync(t => t.trocaId == id);

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
            Estante? estanteId2 = await _context.Estante
                            .Where(x => x.estanteId == 12)
                            .FirstOrDefaultAsync();
            var estanteId = 13;
            EstadoTroca? estadoTroca = await _context.EstadoTroca
                                        .Where(x => x.estadoTrocaId == 1)
                                        .FirstOrDefaultAsync();
            Troca troca = new Troca(dataPedido, 12, estanteId, estadoTroca, _emailSender);
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


        #region GET
        /// <summary>
        /// Funcao apenas para testar. A gestão das troca será feita sempre que 
        ///                             o utilizador adicionar um livro a uma estante de troca ou de desejo
        /// </summary>
        /// <returns></returns>
        [HttpGet("check-match/{estanteId}")]
        public async Task<IActionResult> CheckMatch(int estanteId)
        {
            Troca troca = new Troca();
            var res = await troca.ProcuraMatch(estanteId, _context);

            return Ok(res);
        }

        /// <summary>
        /// Aceita uma troca #TODO ver qual é numero do issue
        /// </summary>
        /// <param name="trocaId"></param>
        /// <returns></returns>
        [HttpPut("aceita-troca/{trocaId}")]
        public async Task<IActionResult> AceitaTroca(int trocaId)
        {
            try
            {
                Troca troca = new Troca();
                var res = await troca.AceitaTroca(trocaId, _context);

                if (res == null)
                {
                    return NotFound("Troca não existe");
                }

                return Ok(res.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// Rejeita uma troca #TODO ver qual é numero do issue
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPut("rejeita-troca/{trocaId}")]
        public async Task<IActionResult> PutRejeitaTroca(int trocaId)
        {
            Troca troca = new Troca();
            var res = await troca.RejeitaTroca(trocaId, _context);

            if (res == null)
            {
                return NotFound("Troca não existe");
            }

            return Ok(res);
        }

        /// <summary>
        /// Cria uma troca direta entre dois utilizadores #TODO ver qual é numero do issue
        /// </summary>
        /// <param name="userName">Username do utilizador que pretende a troca</param>
        /// <param name="estanteDoLivroDesejado">self explanatory</param>
        [HttpPost("solicita-troca-direta/{userId}/{estanteDoLivroDesejado}")]
        public async Task<IActionResult> SolicitaTrocaDireta(int userId, int estanteDoLivroDesejado)
        {
            Troca a = new Troca();

            var existingTroca = await _context.Troca
                .Include(t => t.estadoTroca)
                .Where(t => t.estanteId == estanteDoLivroDesejado && t.Estante2.utilizador.utilizadorID == userId)
                .FirstOrDefaultAsync();

            if (existingTroca != null)
            {
                // Atualiza o estado da troca para "Pendente"
                var estadoPendente = await EstadoTroca.ProcEstadoTroca("Pendente", _context);
                existingTroca.estadoTroca = estadoPendente;
                existingTroca.dataAceiteTroca = null; // Resetar dataAceiteTroca

                try
                {
                    _context.Update(existingTroca);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return StatusCode(500, "Erro ao atualizar o estado da troca existente: " + e.Message);
                }

                return Ok(new { trocaId = existingTroca.trocaId });
            }

            var troca = await a.TrocaDireta(userId, estanteDoLivroDesejado, _context, _emailSender);

            if (troca == null)
            {
                return NotFound("Livro não existe");
            }

            return Ok(new { trocaId = troca.Value.trocaId });
        }


        [HttpGet("check-user-matches/{utilizadorId}")]
        public async Task<IActionResult> CheckUserMatches(int utilizadorId)
        {
            try
            {
                Troca troca = new();
                var res = await troca.ProcuraMatchesParaUtilizador(utilizadorId, _context);
                return Ok(res);
            }
            catch (Exception ex)
            {
                // Log exception (ex) here
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
