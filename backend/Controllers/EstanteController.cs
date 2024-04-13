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

        #region Métodos GET
        /// <summary>
        /// Mostrar uma estante específica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Mostrar todas as estantes de um utilizador
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        // GET: api/Estante/utilizador/{username}
        [HttpGet("utilizador/{username}")]
        public async Task<ActionResult<IEnumerable<Estante>>> GetEstanteByUtilizadorUsername(string username)
        {
            try
            {
                var utilizador = await _context.Utilizador
                .FirstOrDefaultAsync(u => u.username == username);
                if (utilizador == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }

            try
            {
                var estantes = await _context.Estante
                                .Include(e => e.livro)
                                .Include(e => e.livro.generoLivro)
                                .Include(e => e.livro.autorLivro)
                                .Include(e => e.tipoEstante)
                                .Where(e => e.utilizador.username == username
                                && e.livroNaEstante == true)
                                .ToListAsync();
                if (estantes == null)
                {
                    return NotFound();  
                }

                return estantes;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Mostrar um tipo específico de estante
        /// </summary>
        /// <param name="descricaoTipoEstante"></param>
        /// <returns></returns>
        // GET: api/Estante/tipoEstante/{descricaoTipoEstante}
        [HttpGet("tipoEstante/{descricaoTipoEstante}")]
        public async Task<ActionResult<IEnumerable<Estante>>> GetEstanteByTipoEstante(string descricaoTipoEstante)
        {
            try
            {
                var tipoEstante = await _context.TipoEstante
                    .FirstOrDefaultAsync(te => te.descricaoTipoEstante == descricaoTipoEstante);
                if (tipoEstante == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }

            try
            {
                var estantes = await _context.Estante
                                .Include(e => e.livro)
                                .Include(e => e.livro.generoLivro)
                                .Include(e => e.livro.autorLivro)
                                .Where(e => e.tipoEstante.descricaoTipoEstante == descricaoTipoEstante 
                                && e.livroNaEstante == true)
                                .ToListAsync();
                if (estantes == null)
                {
                    return NotFound();
                }
                return estantes;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Mostrar todas as estantes de um utilizador por um tipo específico de estante
        /// </summary>
        /// <param name="username"></param>
        /// <param name="descricaoTipoEstante"></param>
        /// <returns></returns>
        // GET: api/Estante/utilizador/{username}/tipoEstante/{descricaoTipoEstante}
        [HttpGet("utilizador/{username}/tipoEstante/{descricaoTipoEstante}")]
        public async Task<ActionResult<IEnumerable<Estante>>> GetEstanteByUtilizadorAndTipo(string username, string descricaoTipoEstante)
        {
            try
            {
                var utilizador = await _context.Utilizador
                    .FirstOrDefaultAsync(u => u.username == username);
                if (utilizador == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }

            try
            {
                var tipoEstante = await _context.TipoEstante
                    .FirstOrDefaultAsync(te => te.descricaoTipoEstante == descricaoTipoEstante);
                if (tipoEstante == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }

            try
            {
                var estantes = await _context.Estante
                            .Include(e => e.livro)
                            .Include(e => e.livro.generoLivro)
                            .Include(e => e.livro.autorLivro)
                            .Include(e => e.tipoEstante)
                            .Include(e => e.utilizador)
                            .Where(e => e.utilizador.username == username 
                            && e.tipoEstante.descricaoTipoEstante == descricaoTipoEstante
                            && e.livroNaEstante == true)
                            .ToListAsync();
                            
                if (estantes == null)
                {
                    return NotFound();
                }
                return estantes;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Mostrar todas as estantes de um tipo específico que contêm certo livro
        /// </summary>
        /// <param name="tituloLivro"></param>
        /// <param name="descricaoTipoEstante"></param>
        /// <returns></returns>
        // GET: api/Estante/tipoEstante/{descricaoTipoEstante}/livro/{tituloLivro}
        [HttpGet("tipoEstante/{descricaoTipoEstante}/livro/{tituloLivro}")]
        public async Task<ActionResult<IEnumerable<Estante>>> GetEstanteByLivro(string tituloLivro, string descricaoTipoEstante)
        {
            var estantes = await _context.Estante
                            .Include(e => e.livro)
                            .Include(e => e.livro.generoLivro)
                            .Include(e => e.livro.autorLivro)
                            .Include(e => e.tipoEstante)
                        .Where(e => e.livro.tituloLivro.ToLower().Contains(tituloLivro.ToLower())
                        && e.tipoEstante.descricaoTipoEstante == descricaoTipoEstante
                        && e.livroNaEstante == true)
                        .ToListAsync();

            if (estantes == null)
            {
                return NotFound();
            }

            return estantes;
        }
        #endregion

        #region Métodos PUT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="estante"></param>
        /// <returns></returns>
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
        #endregion

        #region Métodos POST
        // POST: api/Estante
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estante>> PostEstante(EstanteCreateDTO estante)
        {
            var novaEstante = new Estante 
            { 
                ultimaAtualizacao = DateTime.Now,
                tipoEstante = await _context.TipoEstante.FindAsync(estante.tipoEstanteId),
                utilizador = await _context.Utilizador.FindAsync(estante.utilizadorId),
                livro = await _context.Livro.FindAsync(estante.livroId),
                livroNaEstante = true
            };

            if (novaEstante == null)
            {
                return BadRequest();
            }

            _context.Estante.Add(novaEstante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstante", new { id = novaEstante.estanteId }, novaEstante);
        }
        #endregion

        #region Métodos DELETE
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
        #endregion

        private bool EstanteExists(int id)
        {
            return _context.Estante.Any(e => e.estanteId == id);
        }
    }
}
