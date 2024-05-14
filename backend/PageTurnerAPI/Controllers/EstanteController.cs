using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var listaEstante = await Estante.PesquisaEstanteBD(_context);

            if(listaEstante == null)
            {
                return NotFound();
            }

            return listaEstante;
        }

        /// <summary>
        /// Mostrar todas as estantes ativas
        /// </summary>
        /// <returns></returns>
        // GET: api/Estante/ativas
        [HttpGet("ativas")]
        public async Task<ActionResult<IEnumerable<EstanteViewDTO>>> GetEstantesAtivas()
        {
            var listaEstante = await Estante.PesquisaEstanteBD(_context);

                if(listaEstante == null)
                {
                    return NotFound();
                }

                var listaFiltrada = listaEstante
                                .Where(estante => estante.livroNaEstante == true)
                                .ToList();

                if (listaFiltrada == null)
                {
                    return NotFound();
                }

                return listaFiltrada;
        }

        // Mostrar uma estante específica
        // GET: api/Estante/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstanteViewDTO>> GetEstante(int id)
        {
            var listaEstante = await Estante.PesquisaEstanteBD(_context);

                if(listaEstante == null)
                {
                    return NotFound();
                }

                var listaFiltrada = listaEstante
                                .FirstOrDefault(estante => estante.estanteId == id);

                if (listaFiltrada == null)
                {
                    return NotFound();
                }

                return listaFiltrada;
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
                var utilizador = Utilizador.UsernameExists(_context, username);
                if (!utilizador)
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
                var listaEstante = await Estante.PesquisaEstanteBD(_context);

                if(listaEstante == null)
                {
                    return NotFound();
                }

                var listaFiltrada = listaEstante.Where(e => e.username == username
                                && e.livroNaEstante == true)
                                .ToList();

                if (listaFiltrada == null)
                {
                    return NotFound();
                }

                return listaFiltrada;
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
                var tipoEstante = TipoEstante.TipoEstanteDescExists(_context, descricaoTipoEstante);
                if (!tipoEstante)
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
                var listaEstante = await Estante.PesquisaEstanteBD(_context);

                if(listaEstante == null)
                {
                    return NotFound();
                }

                var listaFiltrada = listaEstante
                                .Where(e => e.tipoEstante.descricaoTipoEstante == descricaoTipoEstante
                                && e.livroNaEstante == true)
                                .ToList();

                if (listaFiltrada == null)
                {
                    return NotFound();
                }

                return listaFiltrada;
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
                var utilizador = Utilizador.UsernameExists(_context, username);
                if (!utilizador)
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
                var tipoEstante = TipoEstante.TipoEstanteDescExists(_context, descricaoTipoEstante);
                if (!tipoEstante)
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
                var listaEstante = await Estante.PesquisaEstanteBD(_context);

                if(listaEstante == null)
                {
                    return NotFound();
                }

                var listaFiltrada = listaEstante
                                .Where(e => e.username == username
                                && e.tipoEstante.descricaoTipoEstante == descricaoTipoEstante
                                && e.livroNaEstante == true)
                                .ToList();

                if (listaFiltrada == null)
                {
                    return NotFound();
                }

                return listaFiltrada;
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
            var listaEstante = await Estante.PesquisaEstanteBD(_context);

                if(listaEstante == null)
                {
                    return NotFound();
                }

                var listaFiltrada = listaEstante
                            .Where(e => e.livro.tituloLivro.ToLower().Contains(tituloLivro.ToLower())
                            && e.tipoEstante.descricaoTipoEstante == descricaoTipoEstante
                            && e.livroNaEstante == true)
                                .ToList();

                if (listaFiltrada == null)
                {
                    return NotFound();
                }

                return listaFiltrada;
        }
        #endregion

        #region Métodos PUT
        /// <summary>
        /// Método para atualizar uma estante
        /// </summary>
        /// <param name="id"></param>
        /// <param name="estante"></param>
        /// <returns></returns>
        /// PUT: api/Estante/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstante(int id, EstanteUpdateDTO estante)
        {
            if (!Estante.EstanteExists(_context, id))
            {
                return NotFound();
            }

            var atualizacaoSucesso = await Estante.AtualizarEstanteBD(_context, id, estante);
            
            if (!atualizacaoSucesso)
            {
                return NotFound(); // Se a estante não for encontrada ou ocorrer um erro ao atualizar, retornar NotFound
            }

            return NoContent(); // Se a estante for atualizada com sucesso, retornar NoContent
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
            var atualizacaoSucesso = await Estante.AlterarEstadoEstanteBD(_context, estante.estanteId);
            
            if (!atualizacaoSucesso)
            {
                return NotFound(); // Se a estante não for encontrada ou ocorrer um erro ao alterar o estado, retornar NotFound
            }

            return NoContent(); // Se o estado da estante for alterado com sucesso, retornar NoContent
        }
        #endregion

        #region Métodos POST
        /// <summary>
        /// Método para criar uma nova estante
        /// </summary>
        /// <param name="estante"></param>
        /// <returns></returns>
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
            var estanteExistente = await Estante.ObterEstanteExistente(_context, estante.livroId, estante.utilizadorId, estante.tipoEstanteId);

            if (estanteExistente != null && estanteExistente.livroNaEstante == true)
            {
                return BadRequest("O livro já se encontra na estante do utilizador");
            }
            else if (estanteExistente != null && estanteExistente.livroNaEstante == false)
            {
                // Altera o estado da estante existente para "livroNaEstante = true"
                var atualizacaoSucesso = await Estante.AlterarEstadoEstanteBD(_context, estanteExistente.estanteId);

                if (!atualizacaoSucesso)
                {
                    return StatusCode(500, "Ocorreu um erro ao atualizar o estado da estante");
                }

                return CreatedAtAction("GetEstante", new { id = estanteExistente.estanteId }, estanteExistente);
            }
            else
            {
                var novaEstante = await Estante.CriarNovaEstante(_context, estante);

                if (novaEstante == null)
                {
                    return BadRequest();
                }

                return CreatedAtAction("GetEstante", new { id = novaEstante.estanteId }, novaEstante);
            }
        }
        #endregion

        #region Métodos DELETE
        /// <summary>
        /// Método para excluir uma estante
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Estante/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstante(int id)
        {
            var exclusaoSucesso = await Estante.ExcluirEstantePorId(_context, id);
            
            if (!exclusaoSucesso)
            {
                return NotFound(); // Se a estante não for encontrada, retornar NotFound
            }

            return NoContent(); // Se a estante for excluída com sucesso, retornar NoContent
        }
        #endregion

    }
}
