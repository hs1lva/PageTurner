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

        #region Métodos GET        
        /// <summary>
        /// Mostrar todas as estantes
        /// </summary>
        /// <returns></returns>
        // GET: api/Estante
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstanteViewDTO>>> GetEstante()
        {
            var estantes = await _context.Estante
                            .Include(estante => estante.livro)
                            .Include(estante => estante.livro.generoLivro)
                            .Include(estante => estante.livro.autorLivro)
                            .Include(estante => estante.tipoEstante)
                            .Include(estante => estante.utilizador)
                            .Select(estante => new EstanteViewDTO {
                                estanteId = estante.estanteId,
                                ultimaAtualizacao = estante.ultimaAtualizacao,
                                tipoEstante = estante.tipoEstante,
                                livro = estante.livro,
                                livroNaEstante = estante.livroNaEstante,
                                utilizadorId = estante.utilizador.utilizadorID,
                                username = estante.utilizador.username
                            })
                            .ToListAsync();

            return estantes;
        }

        /// <summary>
        /// Mostrar todas as estantes ativas
        /// </summary>
        /// <returns></returns>
        // GET: api/Estante/ativas
        [HttpGet("ativas")]
        public async Task<ActionResult<IEnumerable<EstanteViewDTO>>> GetEstantesAtivas()
        {
            var estantes = await _context.Estante
                            .Include(estante => estante.livro)
                            .Include(estante => estante.livro.generoLivro)
                            .Include(estante => estante.livro.autorLivro)
                            .Include(estante => estante.tipoEstante)
                            .Include(estante => estante.utilizador)
                            .Select(estante => new EstanteViewDTO {
                                estanteId = estante.estanteId,
                                ultimaAtualizacao = estante.ultimaAtualizacao,
                                tipoEstante = estante.tipoEstante,
                                livro = estante.livro,
                                livroNaEstante = estante.livroNaEstante,
                                utilizadorId = estante.utilizador.utilizadorID,
                                username = estante.utilizador.username
                            })
                            .Where(estante => estante.livroNaEstante == true)
                            .ToListAsync();

            return estantes;
        }

        // Mostrar uma estante específica
        // GET: api/Estante/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstanteViewDTO>> GetEstante(int id)
        {
            var estante = await _context.Estante
                            .Include(estante => estante.livro)
                            .Include(estante => estante.livro.generoLivro)
                            .Include(estante => estante.livro.autorLivro)
                            .Include(estante => estante.tipoEstante)
                            .Include(estante => estante.utilizador)
                            .Select(estante => new EstanteViewDTO {
                                estanteId = estante.estanteId,
                                ultimaAtualizacao = estante.ultimaAtualizacao,
                                tipoEstante = estante.tipoEstante,
                                livro = estante.livro,
                                livroNaEstante = estante.livroNaEstante,
                                utilizadorId = estante.utilizador.utilizadorID,
                                username = estante.utilizador.username
                            })
                            .FirstOrDefaultAsync(estante => estante.estanteId == id);
            if (estante == null)
            {
                return NotFound();
            }

            return estante;
        }

        /// <summary>
        /// Mostrar todas as estantes ativas de um utilizador
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        // GET: api/Estante/utilizador/{username}
        [HttpGet("utilizador/{username}")]
        public async Task<ActionResult<IEnumerable<EstanteViewDTO>>> GetEstanteByUtilizadorUsername(string username)
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
                                .Select(e => new EstanteViewDTO {
                                    estanteId = e.estanteId,
                                    ultimaAtualizacao = e.ultimaAtualizacao,
                                    tipoEstante = e.tipoEstante,
                                    livro = e.livro,
                                    livroNaEstante = e.livroNaEstante,
                                    utilizadorId = e.utilizador.utilizadorID,
                                    username = e.utilizador.username
                                })
                                .Where(e => e.username == username
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
        /// Mostrar um tipo específico de estante ativa
        /// </summary>
        /// <param name="descricaoTipoEstante"></param>
        /// <returns></returns>
        // GET: api/Estante/tipoEstante/{descricaoTipoEstante}
        [HttpGet("tipoEstante/{descricaoTipoEstante}")]
        public async Task<ActionResult<IEnumerable<EstanteViewDTO>>> GetEstanteByTipoEstante(string descricaoTipoEstante)
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
                                .Include(e => e.tipoEstante)
                                .Select(e => new EstanteViewDTO {
                                    estanteId = e.estanteId,
                                    ultimaAtualizacao = e.ultimaAtualizacao,
                                    tipoEstante = e.tipoEstante,
                                    livro = e.livro,
                                    livroNaEstante = e.livroNaEstante,
                                    utilizadorId = e.utilizador.utilizadorID,
                                    username = e.utilizador.username
                                })
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
        /// Mostrar todas as estantes de um utilizador por um tipo específico de estante ativa
        /// </summary>
        /// <param name="username"></param>
        /// <param name="descricaoTipoEstante"></param>
        /// <returns></returns>
        // GET: api/Estante/utilizador/{username}/tipoEstante/{descricaoTipoEstante}
        [HttpGet("utilizador/{username}/tipoEstante/{descricaoTipoEstante}")]
        public async Task<ActionResult<IEnumerable<EstanteViewDTO>>> GetEstanteByUtilizadorAndTipo(string username, string descricaoTipoEstante)
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
                                .Select(e => new EstanteViewDTO {
                                    estanteId = e.estanteId,
                                    ultimaAtualizacao = e.ultimaAtualizacao,
                                    tipoEstante = e.tipoEstante,
                                    livro = e.livro,
                                    livroNaEstante = e.livroNaEstante,
                                    utilizadorId = e.utilizador.utilizadorID,
                                    username = e.utilizador.username
                                })
                                .Where(e => e.username == username 
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
        /// Mostrar todas as estantes ativas de um tipo específico que contêm certo livro
        /// </summary>
        /// <param name="tituloLivro"></param>
        /// <param name="descricaoTipoEstante"></param>
        /// <returns></returns>
        // GET: api/Estante/tipoEstante/{descricaoTipoEstante}/livro/{tituloLivro}
        [HttpGet("tipoEstante/{descricaoTipoEstante}/livro/{tituloLivro}")]
        public async Task<ActionResult<IEnumerable<EstanteViewDTO>>> GetEstanteByLivro(string tituloLivro, string descricaoTipoEstante)
        {
                var estantes = await _context.Estante
                                .Include(e => e.livro)
                                .Include(e => e.livro.generoLivro)
                                .Include(e => e.livro.autorLivro)
                                .Include(e => e.tipoEstante)
                                .Select(e => new EstanteViewDTO {
                                    estanteId = e.estanteId,
                                    ultimaAtualizacao = e.ultimaAtualizacao,
                                    tipoEstante = e.tipoEstante,
                                    livro = e.livro,
                                    livroNaEstante = e.livroNaEstante,
                                    utilizadorId = e.utilizador.utilizadorID,
                                    username = e.utilizador.username
                                })
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
        public async Task<IActionResult> PutEstante(int id, EstanteUpdateDTO estante)
        {
            var estanteAtual = await _context.Estante.FindAsync(id);
            if (estanteAtual == null)
            {
                return NotFound();
            }

            //estanteAtual.estanteId = estante.estanteId;
            estanteAtual.ultimaAtualizacao = DateTime.Now;
            estanteAtual.tipoEstante = await _context.TipoEstante.FindAsync(estante.tipoEstanteId);
            estanteAtual.livro = await _context.Livro.FindAsync(estante.livroId);
            estanteAtual.livroNaEstante = estante.livroNaEstante;
            
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

        /// <summary>
        /// Método para alterar o estado de um livro na estante
        /// </summary>
        /// <param name="estante"></param>
        /// <returns></returns>
        // PUT: api/Estante/alterarEstado
        [HttpPut("alterarEstado")]
        public async Task<IActionResult> AlterarEstadoEstante(EstanteUpdateDTO estante)
        {
            var estanteAtual = await _context.Estante.FindAsync(estante.estanteId);
            if (estanteAtual == null)
            {
            return NotFound();
            }

            estanteAtual.livroNaEstante = !estanteAtual.livroNaEstante;
            estanteAtual.ultimaAtualizacao = DateTime.Now;

            try
            {
            await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            if (!EstanteExists(estante.estanteId))
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
            if (estante == null)
            {
                return BadRequest();
            }

            //verificar se já existe uma estante do mesmo tipo com o mesmo livro do mesmo usuário
            var estanteExistente = await _context.Estante
                .FirstOrDefaultAsync(e => e.livro.livroId == estante.livroId 
                                        && e.utilizador.utilizadorID == estante.utilizadorId 
                                        && e.tipoEstante.tipoEstanteId == estante.tipoEstanteId);
            
            if (estanteExistente != null && estanteExistente.livroNaEstante == true)
            {
                return BadRequest("O livro já se encontra na estante do utilizador");
            }
            else if (estanteExistente != null && estanteExistente.livroNaEstante == false)
            {
                estanteExistente.livroNaEstante = true;
                estanteExistente.ultimaAtualizacao = DateTime.Now;
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetEstante", new { id = estanteExistente.estanteId }, estanteExistente);
            }
            else
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
