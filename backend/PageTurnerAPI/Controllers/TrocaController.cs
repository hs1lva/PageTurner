using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PageTurnerAPI;
using PageTurnerAPI.Exceptions;

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
            return await _context.Troca
                            .Include(x => x.estadoTroca)
                            .ToListAsync();
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
            Estante? estanteId2 = await _context.Estante
                            .Where(x => x.estanteId == 12)
                            .FirstOrDefaultAsync();
            var estanteId = 13;
            EstadoTroca? estadoTroca = await _context.EstadoTroca
                                        .Where(x => x.estadoTrocaId == 1)
                                        .FirstOrDefaultAsync();
            Troca troca = new Troca(estanteId2, estanteId, estadoTroca);
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
            try
            {
                var res = await Troca.ProcuraMatch(estanteId, _context);
                return Ok(res);
            }
            catch (TrocaException e){ // Ver com professor. Não está a ser apanhada a exceção, segue para a exceção geral
                
                return NotFound(e.Message);
            }
            catch (Exception e)
            {   
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return NotFound($"Erro desconhecido {e.Message} \n {e.StackTrace}");
            }
        }

        /// <summary>
        /// Aceita uma troca #TODO ver qual é numero do issue
        /// </summary>
        /// <param name="trocaId"></param>
        /// <returns></returns>
        [HttpPut("aceita-troca/{trocaId}")]
        public async Task<IActionResult> AceitaTroca(int trocaId)
        {

            // Procura troca
            var troca = await Troca.GetTrocaById(trocaId, _context);

            var res = await troca.AceitaTroca(trocaId, _context);

            if (res == null)
            {
                return NotFound("Troca não existe");
            }

            return Ok(res);
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
            var troca = await Troca.GetTrocaById(trocaId, _context);

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
        /// <returns></returns>
        [HttpPost("solicita-troca-direta/{userName}/{estanteDoLivroDesejado}")]
        public async Task<IActionResult> SolicitaTrocaDireta(string userName, int estanteDoLivroDesejado)
        {

            var trocaInicial = await Troca.CriaTroca(userName, estanteDoLivroDesejado, _context);

            var troca = await trocaInicial.TrocaDireta(trocaInicial, _context);

            if (troca == null)
            {
                return NotFound("Livro não existe");
            }

            return Ok(troca);

        }
    }
}
