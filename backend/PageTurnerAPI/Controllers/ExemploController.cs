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

        private readonly PageTurnerContext _Db;//--> serviço de acesso ao BD
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
        public ExemploController(PageTurnerContext db
                                , IEmailSender emailSender)
        {
            _Db = db;
            _emailSender = emailSender;
        }
        #endregion

        #region MetodosGet


        /// <summary>
        /// É um forma diferente de fazer o mesmo que a função em cima.
        /// neste caso a ligação à BD está no modelo. 
        /// </summary>
        /// <returns></returns>
        [HttpGet("/outro-destino")]
        public async Task<IActionResult> GetOutroDestino()
        {
            var lista = await Pais.VerTodosPaises(_Db);
            if (lista == null)
            {
                return NotFound();
            }
            return Ok(lista);
        }




        #endregion

        #region MetodosPost
        /// <summary>
        /// Apenas para testar e vermos como podemos organizar o código
        /// </summary>
        /// <param name="nomePais"></param>
        /// <returns></returns>
        [HttpPost("/criar-pais/{nomePais}")]
        public async Task<IActionResult> PostCriarPais(string nomePais)
        {
            try
            {
                var pais = new Pais(nomePais, _Db);
                _Db.Pais.Add(pais);
                await _Db.SaveChangesAsync();

                return Ok(pais);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomeCidade"></param>
        /// <returns></returns>
        [HttpPost("/criar-cidade/{nomeCidade}")]
        public async Task<IActionResult> PostCriarCidade(string nomeCidade)
        {
            try
            {
                var cidade = new Cidade(nomeCidade, _Db);
                _Db.Cidade.Add(cidade);
                await _Db.SaveChangesAsync();

                return Ok(cidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        #endregion


    }
}
