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
using System.Net;
using PageTurnerAPI.Services;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        // Construtor privado vazio para resolver o problema do ASP.NET Core
        private UtilizadorController()
        {
        }

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

            var estanteDesejos = await _context.Estante
                .Include(e => e.livro)
                .Include(e => e.tipoEstante)
                .Where(e => e.utilizador.utilizadorID == id && e.tipoEstante.descricaoTipoEstante == TipoEstantes.Desejos && e.livroNaEstante)
                .Select(e => new
                {
                    e.estanteId,
                    livro = new
                    {
                        e.livro.livroId,
                        e.livro.tituloLivro,
                        e.livro.capaLarge,
                        e.livro.autorLivro
                    },
                    e.tipoEstante,
                    e.utilizador.utilizadorID,
                })
                .ToListAsync();

            var estanteTroca = await _context.Estante
                .Include(e => e.livro)
                .Include(e => e.tipoEstante)
                .Where(e => e.utilizador.utilizadorID == id && e.tipoEstante.descricaoTipoEstante == TipoEstantes.Troca && e.livroNaEstante)
                .Select(e => new
                {
                    e.estanteId,
                    livro = new
                    {
                        e.livro.livroId,
                        e.livro.tituloLivro,
                        e.livro.capaLarge,
                        e.livro.autorLivro
                    },
                    e.tipoEstante,
                    e.utilizador.utilizadorID,
                })
                .ToListAsync();

            var estantePessoal = await _context.Estante
                .Include(e => e.livro)
                .Include(e => e.tipoEstante)
                .Where(e => e.utilizador.utilizadorID == id && e.tipoEstante.descricaoTipoEstante == TipoEstantes.Pessoal && e.livroNaEstante)
                .Select(e => new
                {
                    e.estanteId,
                    livro = new
                    {
                        e.livro.livroId,
                        e.livro.tituloLivro,
                        e.livro.capaLarge,
                        e.livro.autorLivro
                    },
                    e.tipoEstante,
                    e.utilizador.utilizadorID,
                })
                .ToListAsync();


            // Agora, combinamos os resultados manualmente
            var resultado = new
            {
                Utilizador = utilizador,
                EstanteTroca = estanteTroca,
                EstanteDesejos = estanteDesejos,
                EstantePessoal = estantePessoal
            };

            return Ok(resultado);
        }


        /// <summary>
        /// Obter um utilizador pelo username
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet("nome/{nome}")] // -- Issue #39
        public async Task<ActionResult<Utilizador>> GetUtilizadorNome(string nome)
        {
            var utilizador = await _context.Utilizador.FirstOrDefaultAsync(u => u.username == nome);

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

        [HttpGet("ConfirmarEmail/{id}")]
        public async Task<IActionResult> ConfirmarEmail(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            // Verificar se o email já foi confirmado
            if (utilizador.dataRegisto != null)
            {
                return Conflict("O email já foi confirmado anteriormente.");
            }

            // Confirmar o email atualizando a data de registo
            utilizador.dataRegisto = DateTime.Now;
            await _context.SaveChangesAsync();

            // Redirecionar para a página de login
            return Redirect("http://localhost:3000/login");
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
        /// <param name="utilizadorDTO"></param>
        /// <returns></returns>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostUtilizador(UtilizadorCreateDTO utilizadorDTO)
        {
            try
            {
                // Definir o estado de conta como "Ativo" por padrão
                utilizadorDTO.estadoContaId = 1; // 1 - Ativo

                // Verificar se o tipo de utilizador é válido
                if (!TipoUtilizador.IsValidTipoUtilizador(utilizadorDTO.tipoUtilizadorId))
                {
                    return BadRequest("Tipo de utilizador inválido. Deve ser 1 (Administrador) ou 2 (Utilizador).");
                }

                // Verificar se o username já existe
                if (Utilizador.UsernameExists(_context, utilizadorDTO.username))
                {
                    return Conflict("O username já está em uso.");
                }

                // Verificar se o email já existe
                if (Utilizador.EmailExists(_context, utilizadorDTO.email))
                {
                    return Conflict("O email já está em uso.");
                }

                // Gerar o hash da senha
                string hashedPassword = Utilizador.HashPassword(utilizadorDTO.password);

                var utilizador = new Utilizador
                {
                    nome = utilizadorDTO.nome,
                    apelido = utilizadorDTO.apelido,
                    dataNascimento = utilizadorDTO.dataNascimento,
                    username = utilizadorDTO.username,
                    password = hashedPassword, // Atribuir o hash da senha
                    email = utilizadorDTO.email,
                    fotoPerfil = utilizadorDTO.fotoPerfil,
                    dataRegisto = null, // DataRegisto é preenchido NULL inicialmente
                    ultimologin = utilizadorDTO.ultimologin,
                    notficacaoPedidoTroca = utilizadorDTO.notficacaoPedidoTroca,
                    notficacaoAceiteTroca = utilizadorDTO.notficacaoAceiteTroca,
                    notficacaoCorrespondencia = utilizadorDTO.notficacaoCorrespondencia,
                    tipoUtilizadorId = utilizadorDTO.tipoUtilizadorId,
                    estadoContaId = utilizadorDTO.estadoContaId,
                    cidadeId = utilizadorDTO.cidadeId
                };

                // Adicionar o utilizador ao contexto
                _context.Utilizador.Add(utilizador);
                // Salvar as alterações no contexto
                _context.SaveChanges();

                // Enviar email de confirmação assíncrono
                EmailSender emailSender = new EmailSender(_context);
                emailSender.SendEmailConfirmationAsync(utilizador.email, utilizador.utilizadorID);

                return CreatedAtAction("GetUtilizador", new { id = utilizador.utilizadorID }, utilizador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar utilizador: {ex.Message}");
            }
        }

        /// <summary>
        /// Autenticar um utilizador
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {


                var user = await Utilizador.GetUtilizadorByLoginDTO(loginDTO, _context);
                if (user == null)
                {
                    return Unauthorized("Dados invalidos."); // Utilizador incorreto
                }




                // Verificar se a password está correta
                var pass = Utilizador.CheckPassword(loginDTO.Password, user.password);
                if (!pass.Result)
                {
                    return Unauthorized("Dados invalidos."); // Password incorreta
                }





                var token = Utilizador.Login(user);
                if (token.ToString().IsNullOrEmpty())
                {
                    return Unauthorized("Credenciais inválidas.");
                }
                var resp = new { Token = token };

                // Response.Headers["Authorization"] = $"Bearer {token}";// Adicionar o token ao cabeçalho da resposta

                return Ok(resp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao fazer login: {ex.Message}");
            }
        }

        /// <summary>
        /// Logout de um utilizador autenticado.
        /// </summary>
        /// <returns>Um IActionResult que representa o resultado do logout.</returns>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Efetuar o logout do esquema de autenticação por cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpar a cookie de autenticação do cliente
            foreach (var cookieKey in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookieKey);
            }

            return Ok("Logout efetuado com sucesso.");
        }


        /// <summary>
        /// Desativar a conta do utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/Desativar")]
        public async Task<ActionResult> DesativarConta(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound("Utilizador não encontrado.");
            }

            // Define o estado da conta como 2 (Inativo)
            utilizador.estadoContaId = 2;

            try
            {
                await _context.SaveChangesAsync();

                // Enviar e-mail de notificação de desativação da conta ao utilizador
                EmailSender emailSender = new EmailSender(_context);
                await emailSender.SendAccountDeactivationEmailAsync(utilizador.email);

                return Ok("Conta desativada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao desativar conta: {ex.Message}");
            }
        }

        /// <summary>
        /// Reativar a conta do utilizador pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/Reativar")]
        public async Task<ActionResult> ReativarConta(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound("Utilizador não encontrado.");
            }

            // Define o estado da conta como 1 (Ativo)
            utilizador.estadoContaId = 1;

            try
            {
                await _context.SaveChangesAsync();

                // Enviar e-mail de notificação de reativação da conta ao utilizador
                EmailSender emailSender = new EmailSender(_context);
                await emailSender.SendAccountReactivationEmailAsync(utilizador.email);

                return Ok("Conta reativada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao reativar conta: {ex.Message}");
            }
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


        // ---------------------------------------------------------------------
        // ------------- METODOS PARA O ADMINISTRADOR --------------------------
        // ---------------------------------------------------------------------

        #region Métodos GET Admin

        /// <summary>
        /// Obter todos os utilizadores (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/Utilizadores")]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizador_Admin()
        {
            return await _context.Utilizador.ToListAsync();
        }

        /// <summary>
        /// Obter todos os utilizadores banidos (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/Banidos")]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizadorBanidos_Admin()
        {
            var utilizadoresBanidos = await _context.Utilizador.Where(u => u.estadoContaId == 3).ToListAsync();

            if (!utilizadoresBanidos.Any())
            {
                return NotFound("Não há utilizadores banidos.");
            }

            return utilizadoresBanidos;
        }

        /// <summary>
        /// Obter todos os países dos utilizadores (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/PaisesUtilizadores")]
        public async Task<ActionResult<IEnumerable<string>>> GetPaisesUtilizadores_Admin()
        {
            var paisesUtilizadores = await Utilizador.ObterPaisesUtilizadores(_context);

            if (!paisesUtilizadores.Any())
            {
                // Mensagem de erro personalizada
                return NotFound("Não foram encontrados países de utilizadores.");
            }

            return Ok(paisesUtilizadores); // Retorna a lista de países encontrados
        }


        /// <summary>
        /// Obter todos os comentários aos livros (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("Admin/ComentariosGerais")]
        public async Task<ActionResult<IEnumerable<ComentarioLivro>>> GetComentariosGerais_Admin()
        {
            return await _context.ComentarioLivro.ToListAsync();
        }

        #endregion

        #region Métodos PUT Admin

        /// <summary>
        /// Banir um utilizador pelo ID (Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Admin/Banir/{id}")]
        public async Task<IActionResult> BanirUtilizador_Admin(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            utilizador.estadoContaId = 3; // Define o estado da conta como "Banido"
            await _context.SaveChangesAsync();

            try
            {
                // Enviar e-mail de notificação de banimento da conta
                EmailSender emailSender = new EmailSender(_context);
                await emailSender.SendAccountBanEmailAsync(utilizador.email);

                return NoContent(); // Retorna um status 204 para indicar que a operação foi bem-sucedida
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao banir utilizador: {ex.Message}");
            }
        }


        /// <summary>
        /// Atualizar alguns campos do utilizador pelo username (Admin)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="utilizadorDTO"></param>
        /// <returns></returns>
        [HttpPut("Admin/Username/{username}")]
        public async Task<IActionResult> PutUtilizadorByUsername_Admin(string username, UtilizadorUpdateDTO utilizadorDTO)
        {
            var userToUpdate = await _context.Utilizador.FirstOrDefaultAsync(u => u.username == username);

            if (userToUpdate == null)
            {
                return NotFound();
            }

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

            return NoContent();
        }

        /// <summary>
        /// Atualizar alguns campos do utilizador pelo ID (Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="utilizadorDTO"></param>
        /// <returns></returns>
        [HttpPut("Admin/Id/{id}")]
        public async Task<IActionResult> PutUtilizadorById_Admin(int id, UtilizadorUpdateDTO utilizadorDTO)
        {
            var userToUpdate = await _context.Utilizador.FindAsync(id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

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

            return NoContent();
        }

        #endregion
    }
}
