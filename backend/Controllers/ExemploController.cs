using System.ComponentModel.DataAnnotations;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Services;
using backend.Interface;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExemploController : ControllerBase
    {
        private readonly PageTurnerContext _context;
        private readonly IEmailSender _emailSender;


        List<string> listaEmails = new List<string>(
            new string[] { "a24204@alunos.ipca.pt", "a16368@alunos.ipca.pt",
                         "a26339@alunos.ipca.pt","a24200@alunos.ipca.pt",
                         "a26342@alunos.ipca.pt"}

        );


        public ExemploController(PageTurnerContext context
                                , IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
//        public IActionResult Get() //-> a diferença entre e o metodo em Task<IActionResult> é o tipo de dados que pode retornar. 
                                            // o uso do async e await que torna a API mais rapida, nao fica à espera do servidor (BD, neste caso) para retornar os dados.
        public async Task<IActionResult> Get()
        {
            ////Podemos fazer assim:
            var lista = _context.Pais.ToList();

            return Ok(lista);
            ////ou assim:

            // return Ok(_context.Pais.ToList()); // --> apenas para o IActionResult
        }

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




    }
}
