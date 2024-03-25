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
    public class UtilizadorController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public UtilizadorController(PageTurnerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter todos os utilizadores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizador()
        {
            // Validar se o utilizador tem dataRegisto != null (confirmou o email)
            return await _context.Utilizador.Where(x=>x.dataRegisto != default(DateTime)).ToListAsync();
        }

        /// <summary>
        /// Obter um utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizador>> GetUtilizador(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);

            if (utilizador == null)
            {
                return NotFound();
            }

            // Se a data de registo for NULL, o utilizador ainda não confirmou o email
            if (utilizador.dataRegisto == default(DateTime))
            {
                return BadRequest("Utilizador com email não confirmado.");
            }

            return utilizador;
        }

        /// <summary>
        /// Obter um utilizador pelo nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<Utilizador>> GetUtilizadorNome(string nome)
        {
            var utilizador = await _context.Utilizador.FirstOrDefaultAsync(u => u.nome == nome);

            if (utilizador == null)
            {
                return NotFound();
            }

            // Se a data de registo for NULL, o utilizador ainda não confirmou o email
            if (utilizador.dataRegisto == default(DateTime))
            {
                return BadRequest("Utilizador com email não confirmado.");
            }

            return utilizador;
        }

        /// <summary>
        /// Atualizar um utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="utilizador"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizador(int id, [FromBody] Utilizador utilizador)
        {
            var userToUpdate = await _context.Utilizador.FindAsync(id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Impedir a alteração do email -- Issue #40
            if (utilizador.email != userToUpdate.email)
            {
                return BadRequest("Não é permitido alterar o email.");
            }

            try
            {
                // Atualizar os campos permitidos -- Issue #37
                if (!string.IsNullOrEmpty(utilizador.nome))
                {
                    // Verificar se o nome é diferente do atual
                    if (utilizador.nome != userToUpdate.nome)
                    {
                        await userToUpdate.AtualizarNomeAsync(utilizador.nome, _context);
                    }
                }

                if (!string.IsNullOrEmpty(utilizador.apelido))
                {
                    // Verificar se o apelido é diferente do atual
                    if (utilizador.apelido != userToUpdate.apelido)
                    {
                        await userToUpdate.AtualizarApelidoAsync(utilizador.apelido, _context);
                    }
                }

                if (utilizador.dataNascimento != default(DateTime))
                {
                    // Verificar se a data de nascimento é diferente da atual
                    if (utilizador.dataNascimento != userToUpdate.dataNascimento)
                    {
                        await userToUpdate.AtualizarDataNascimentoAsync(utilizador.dataNascimento, _context);
                    }
                }   

                if (!string.IsNullOrEmpty(utilizador.fotoPerfil))
                {
                    // Verificar se a foto de perfil é diferente da atual
                    if (utilizador.fotoPerfil != userToUpdate.fotoPerfil)
                    {
                        await userToUpdate.AtualizarFotoPerfilAsync(utilizador.fotoPerfil, _context);
                    }
                }
                
                return Ok("Alterado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar utilizador: {ex.Message}");
            }
        }

        /// <summary>
        /// Confirmar o email do utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ConfirmarEmail/{id}")]
        public async Task<ActionResult<Utilizador>> ConfirmarEmail(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);

            // Chamar funcao para confirmar email
            // validar se é null
            if (utilizador == null)
            {
                return NotFound();
            }
            else
            {
                await utilizador.AtualizarDataRegistoAsync(DateTime.Now, _context);

                return Ok("Email confirmado com sucesso.");
            }
        }

        /// <summary>
        /// Criar um novo utilizador
        /// </summary>
        /// <param name="utilizador"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilizador>> PostUtilizador(Utilizador utilizador)
        {
            // Campo DataRegisto é preenchido NULL para conseguirmos verificar a confirmação do email
            utilizador.dataRegisto = default(DateTime);

            _context.Utilizador.Add(utilizador);
            await _context.SaveChangesAsync();

            // Enviar email de confirmação
            EmailSender emailSender = new EmailSender();
            emailSender.SendEmailConfirmationAsync(utilizador.email, utilizador.utilizadorID);

            return CreatedAtAction("GetUtilizador", new { id = utilizador.utilizadorID }, utilizador);
        }

        /// <summary>
        /// Eliminar um utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizador(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            _context.Utilizador.Remove(utilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verificar se um utilizador existe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool UtilizadorExists(int id)
        {
            return _context.Utilizador.Any(e => e.utilizadorID == id);
        }
    }
}
