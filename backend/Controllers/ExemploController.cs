using System.ComponentModel.DataAnnotations;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Interface;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExemploController : ControllerBase
    {
        #region Atributos

        //Os serviços foram criados de formas diferentes, depois temos de ver o que é o melhor para fazermos sempre igual

        private readonly PageTurnerContext _context;//--> serviço de acesso ao BD
        private readonly IEmailSender _emailSender;// --> servico de envio de email
        private List<string> listaEmails = new List<string>(
            new string[] { "a24204@alunos.ipca.pt", "a16368@alunos.ipca.pt",
                         "a26339@alunos.ipca.pt","a24200@alunos.ipca.pt",
                         "a26342@alunos.ipca.pt"} // ---> criei apenas para testar o envio de varios emails

        );
        #endregion

        #region Construtores
        //Construtor
        /// <summary>
        /// O construtor abre com os serviços necessarios.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="emailSender"></param>
        public ExemploController(PageTurnerContext context
                                , IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        #endregion

        #region MetodosGet
        /// <summary>
        /// Faz get de tudo, retorna uma lista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
//        public IActionResult Get() //-> a diferença entre e o metodo em Task<IActionResult> é o tipo de dados que pode retornar. 
                                            // o uso do async e await que torna a API mais rapida, nao fica à espera do servidor (BD, neste caso) para retornar os dados.
        public async Task<IActionResult> Get()
        {
            ////Podemos fazer assim:
            var lista = _context.Pais.ToList();

            if(lista == null)
            {
                return NotFound();
            }

            return Ok(lista);
            ////ou assim:

            // return Ok(_context.Pais.ToList()); // --> apenas para o IActionResult
        }

        /// <summary>
        /// É um forma diferente de fazer o mesmo que a função em cima.
        /// neste caso a ligação à BD está no modelo. 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/outroDestino")]
        public async Task<IActionResult> GetOutroDestino()
        {
            Pais a = new(_context);
            var lista = await a.VerTodosPaises();
            if (lista == null)
            {
                return NotFound();
            }
            return Ok(lista);
        }

        /// <summary>
        /// Retorna um objeto Pais pelo nome
        /// </summary>
        /// <param name="nomePais"></param>
        /// <returns>objeto Pais</returns>
        [HttpGet("{nomePais}")]
        public async Task<IActionResult> GetById(string nomePais)
        {
            var pais = await _context.Pais.FirstOrDefaultAsync(x => x.nomePais == nomePais);
            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }



        #endregion

        #region MetodosPost

        /// <summary>
        /// Cria novo Pais, envia email aos administradores definidos na listaEmails
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Retorna o novo Pais, envia email aos administradores</returns>
        [HttpPost]
        public async Task<ActionResult<Pais>> PostInserirPais([FromBody] string p)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try{

                var a = new Pais(p, _context);
                _context.Pais.Add(a);
                await _context.SaveChangesAsync();
                foreach (var email in listaEmails)
                {
                    await _emailSender.SendEmailAsync(email, "Administrador Pageturner",
                                                     "Foi Criado um novo pais: " + a.nomePais);
                }
                return a;
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }

        }

        #endregion


    }
}
