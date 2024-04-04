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

        /// <summary>
        /// Faz a procura de um livro em qql estante issue 74
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-lista-users")]
        public async Task<IActionResult> GetUser()
        {
            //apenas para testar
            string estanteProcura = "Estante Desejos";
            int livroId = 3;

            //verifica se a estante existe
            // TODO substituir por funcao de estante
            Estante? estanteExiste = await _context.Estante
                .Include(x => x.tipoEstante)
                .Where(x => x.tipoEstante.descricaoTipoEstante == estanteProcura)
                .FirstOrDefaultAsync();

            if (estanteExiste == null)
            {
                return NotFound("Estante não existe");
            }

            //verifica se o livro existe na bd 
            // TODO substituir por funcao de livro
            Livro? livr = await _context.Livro
                .Where(x => x.livroId == livroId)
                .FirstOrDefaultAsync();
            
            if (livr == null)
            {
                return NotFound("Livro não existe");
            }

            Troca b = new Troca();
            var res = await b.ProcuraLivroEmEstante(livroId, estanteProcura, _context);
            if (res == null)
            {
                return NotFound("Livro não existe em nenhuma estante");
            }

            // verifica se utilizadores tem o livros na estante de 

            // envia email com a lista de utilizadores que tem o livro
            

            return Ok(res);
        }

        /// <summary>
        /// Rejeita uma troca issue 75
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
        /// Cria uma troca direta entre dois utilizadores issue 74
        /// </summary>
        /// <param name="userName">Username do utilizador que pretende a troca</param>
        /// <param name="estanteDoLivroDesejado">self explanatory</param>
        /// <returns></returns>
        [HttpPost("solicita-troca-direta/{userName}/{estanteDoLivroDesejado}")]
        public async Task<IActionResult> SolicitaTrocaDireta(string userName, int estanteDoLivroDesejado){
            Troca a = new Troca();

            var troca = await a.TrocaDireta(userName, estanteDoLivroDesejado, _context);

            if (troca == null)
            {
                return NotFound("Livro não existe");
            }

            return Ok(troca);

        }
    }
}
