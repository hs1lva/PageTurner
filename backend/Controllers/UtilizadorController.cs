using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        #region Métodos GET

        /// <summary>
        /// Obter todos os utilizadores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizador()
        {
            // Validar se o utilizador tem dataRegisto != null (confirmou o email)
            // @TODO: fazer isto no get?
            // return await _context.Utilizador.Where(x => x.dataRegisto != null).ToListAsync();
            return await _context.Utilizador.ToListAsync();
        }

        /// <summary>
        /// Obter um utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizador>> GetUtilizador(int id)
        {
            var utilizador = await _context.Utilizador
                .Include(u => u.Comentarios)
                .ThenInclude(c => c.estadoComentario)
                .Include(u => u.Avaliacoes)
                .FirstOrDefaultAsync(u => u.utilizadorID == id);

            if (utilizador == null)
            {
                return NotFound();
            }

            // @TODO: faz sentido no get?
            // Se a data de registo for NULL, o utilizador ainda não confirmou o email
            //if (utilizador.dataRegisto == null)
            //{
            //    return BadRequest("Utilizador com email não confirmado.");
            // }

            return utilizador;
        }

        /// <summary>
        /// Obter um utilizador pelo nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet("nome/{nome}")] // -- Issue #39
        public async Task<ActionResult<Utilizador>> GetUtilizadorNome(string nome)
        {
            var utilizador = await _context.Utilizador.FirstOrDefaultAsync(u => u.nome == nome);

            if (utilizador == null)
            {
                return NotFound();
            }

            // Se a data de registo for NULL, o utilizador ainda não confirmou o email
            // @TODO: faz sentido no get?
            //if (utilizador.dataRegisto == null)
            //{
            //  return BadRequest("Utilizador com email não confirmado.");
            //}

            return utilizador;
        }

        /// <summary>
        /// Confirmar o email do utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ConfirmarEmail/{id}")] // -- Issue #42
        public async Task<ActionResult<Utilizador>> ConfirmarEmail(int id)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao confirmar email: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint para autenticação com a Google
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExternalLogin")] // -- Issue #41
        public IActionResult ExternalLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(ExternalLoginCallback)) };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Callback da autenticação com a Google
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExternalLoginCallback")] // -- Issue #41
        public async Task<IActionResult> ExternalLoginCallback()
        {
            // Obter os dados do utilizador autenticado pela Google
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                // Se a autenticação não for bem-sucedida, redirecionar ou retornar um erro
                return BadRequest("Falha na autenticação com a Google.");
            }

            // Criar as claims do utilizador
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,
                    authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                new Claim(ClaimTypes.Name, authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value),
                // Podes adicionar outras claims conforme necessário
            };

            // Criar a identidade do utilizador
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Autenticar o utilizador no esquema de autenticação por cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(userIdentity));

            // Redirecionar para a página principal ou para onde quer que o utilizador deva ser redirecionado após autenticação
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Métodos PUT

        /// <summary>
        /// Atualizar um utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="utilizador"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizador(int id, [FromBody] UtilizadorUpdateDTO utilizadorDTO)
        {
            var userToUpdate = await _context.Utilizador.FindAsync(id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Impedir a alteração do email -- Issue #40
            //if (utilizadorDTO.email != userToUpdate.email)
            //{
            //    return BadRequest("Não é permitido alterar o email.");
            //}

            try
            {
                // Atualizar os campos permitidos
                if (!string.IsNullOrEmpty(utilizadorDTO.Nome) && utilizadorDTO.Nome != userToUpdate.nome)
                {
                    userToUpdate.nome = utilizadorDTO.Nome;
                }

                if (!string.IsNullOrEmpty(utilizadorDTO.Apelido) && utilizadorDTO.Apelido != userToUpdate.apelido)
                {
                    userToUpdate.apelido = utilizadorDTO.Apelido;
                }

                if (utilizadorDTO.DataNascimento.HasValue &&
                    utilizadorDTO.DataNascimento != userToUpdate.dataNascimento)
                {
                    userToUpdate.dataNascimento = utilizadorDTO.DataNascimento.Value;
                }

                if (!string.IsNullOrEmpty(utilizadorDTO.FotoPerfil) &&
                    utilizadorDTO.FotoPerfil != userToUpdate.fotoPerfil)
                {
                    userToUpdate.fotoPerfil = utilizadorDTO.FotoPerfil;
                }

                await _context.SaveChangesAsync();

                return Ok("Alterado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar utilizador: {ex.Message}");
            }
        }
    

        /// <summary>
        /// Atualizar a senha do utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="novaSenha"></param>
        /// <returns></returns>
        [HttpPut("{id}/AlterarSenha")]
        public async Task<IActionResult> AlterarSenha(int id, [FromBody] UtilizadorUpdateSenhaDTO alterarSenhaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToUpdate = await _context.Utilizador.FindAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                await userToUpdate.AtualizarSenhaAsync(alterarSenhaDTO.NovaSenha, _context);
                return Ok("Senha alterada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar senha: {ex.Message}");
            }
        }


        #endregion

        #region Métodos POST

        /// <summary>
        /// Criar um novo utilizador
        /// </summary>
        /// <param name="utilizador"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilizador>> PostUtilizador(UtilizadorCreateDTO utilizadorDTO)
        {
            var utilizador = new Utilizador
            {
                nome = utilizadorDTO.nome,
                apelido = utilizadorDTO.apelido,
                dataNascimento = utilizadorDTO.dataNascimento,
                username = utilizadorDTO.username,
                password = utilizadorDTO.password,
                email = utilizadorDTO.email,
                fotoPerfil = utilizadorDTO.fotoPerfil,
                dataRegisto = null, // DataRegisto é preenchido NULL inicialmente
                ultimologin = utilizadorDTO.ultimologin,
                notficacaoPedidoTroca = utilizadorDTO.notficacaoPedidoTroca,
                notficacaoAceiteTroca = utilizadorDTO.notficacaoAceiteTroca,
                notficacaoCorrespondencia = utilizadorDTO.notficacaoCorrespondencia,
                tipoUtilizadorId = utilizadorDTO.tipoUtilizadorId,
                estadoContaId = utilizadorDTO.estadoContaId
            };

            try
            {
                _context.Utilizador.Add(utilizador);
                await _context.SaveChangesAsync();

                EmailSender emailSender = new EmailSender();
                await emailSender.SendEmailConfirmationAsync(utilizador.email, utilizador.utilizadorID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar utilizador: {ex.Message}");
            }

            return CreatedAtAction("GetUtilizador", new { id = utilizador.utilizadorID }, utilizador);
        }


        #endregion

        #region Métodos DELETE

        /// <summary>
        /// Eliminar um utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")] // -- Issue #38
        public async Task<IActionResult> DeleteUtilizador(int id)
        {
            try
            {
                var utilizador = await _context.Utilizador.FindAsync(id);

                if (utilizador == null)
                {
                    return NotFound();
                }

                _context.Utilizador.Remove(utilizador);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao eliminar utilizador: {ex.Message}");
            }

            return NoContent();
        }

        #endregion
        
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
