using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;

/*public static List<LivroDTO> PesquisaLivroBd(string termo, PageTurnerContext context) { ... }*/
namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public LivroController(PageTurnerContext context)
        {
            _context = context;
        }

        #region Métodos GET
        // GET: api/Livro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivro()
        {
            return await _context.Livro.ToListAsync();
        }

        // GET: api/Livro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LivroDTO>> GetLivro(int id)
        {
            var livro = await _context.Livro
                .Include(x => x.Comentarios)
                .Include(x => x.Avaliacoes)
                .FirstOrDefaultAsync(x => x.livroId == id);

            if (livro == null)
            {
                return NotFound();
            }

            // usamos o DTO para incluir a media de avaliação
            var livroDto = new LivroDTO
            {
                LivroId = livro.livroId,
                TituloLivro = livro.tituloLivro,
                AnoPrimeiraPublicacao = livro.anoPrimeiraPublicacao,
                IdiomaOriginalLivro = livro.idiomaOriginalLivro,
                AutorLivro = livro.autorLivro,
                GeneroLivro = livro.generoLivro,
                MediaAvaliacao = livro.MediaAvaliacao(),
                Comentarios = livro.Comentarios,
                Avaliacoes = livro.Avaliacoes
            };

            return livroDto;
        }


        /// <summary>
        /// Método para pesquisar um livro pelo título
        /// </summary>
        /// <param name="termo"></param>
        /// <returns></returns>
        // GET: api/Livro/Pesquisar/{termo}
        [HttpGet("PesquisarLivro/{termo}")]
        public async Task<ActionResult<List<LivroDTO>>> PesquisarLivro(string termo)
        {
            var livros = await Livro.PesquisaLivroBd(termo, _context);

            if (livros == null)
            {
                return NotFound();
            }

            List<LivroDTO> livroDto = new List<LivroDTO>();
            foreach (var livro in livros)
            {
                livroDto.Add(new LivroDTO
                {
                    LivroId = livro.LivroId,
                    TituloLivro = livro.TituloLivro,
                    AutorLivro = livro.AutorLivro,
                    GeneroLivro = livro.GeneroLivro
                });
            }

            return livros;
        }

        // GET: api/Livro/Pesquisar/OpenLibrary/{titulo}
        [HttpGet("PesquisarLivro/OpenLibrary/{titulo}")]
        public async Task<string> PesquisarLivroOL(string titulo)
        {
            var servicoAPI = new Services.ServicoAPI();
            //var url = servicoAPI.ProcurarLivro(titulo);
            var livros = await servicoAPI.BuscarLivrosPorTitulo(titulo);

            if (livros == null)
            {
                return NotFound().ToString();
            }

            return livros;
        }

        /// <summary>
        /// Método para obter o perfil de um livro através do ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Livro/GetPerfilLivro/{id}
        [HttpGet("GetPerfilLivro/{id}")]
        public async Task<ActionResult<Livro>> PerfilLivro(int id)
        {
            try
            {
                var livro = await _context.Livro
                                            .Include(l => l.autorLivro)
                                            .Include(l => l.generoLivro)
                                            .Include(l => l.Avaliacoes)
                                            .FirstOrDefaultAsync(l => l.livroId == id);

                if (livro == null)
                {
                    return NotFound();
                }

                var comentarios = await _context.ComentarioLivro
                                                .Where(c => c.livroId == id)
                                                .ToListAsync();

                if (comentarios.Any())
                {
                    livro.Comentarios = comentarios;
                }

                return livro;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter detalhes do livro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método para sugestão de livros com base nos autores e géneros dos livros da estante do utilizador
        /// </summary>
        /// <param name="utilizadorId"></param>
        /// <param name="tipoEstante"></param>
        /// <returns></returns>
        [HttpGet("SugerirLivros/{utilizadorId}")]
        public async Task<ActionResult<List<LivroDTO>>> SugerirLivros(int utilizadorId)
        {
            try
            {
                var livro = new Livro();
                var livrosSugeridosDTO = await livro.SugerirLivros(utilizadorId, _context);
                return livrosSugeridosDTO;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao sugerir livros: " + ex.Message);
            }
        }

        #endregion

        #region Métodos PUT
        // PUT: api/Livro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivro(int id, Livro livro)
        {
            if (id != livro.livroId)
            {
                return BadRequest();
            }

            _context.Entry(livro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(id))
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
        // POST: api/Livro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Livro>> PostLivro(LivroCreateDTO livroDto)
        {
            var livro = new Livro
            {
                tituloLivro = livroDto.TituloLivro,
                anoPrimeiraPublicacao = livroDto.AnoPrimeiraPublicacao,
                idiomaOriginalLivro = livroDto.IdiomaOriginalLivro,
                autorLivro = await _context.AutorLivro.FindAsync(livroDto.AutorLivroId),
                generoLivro = await _context.GeneroLivro.FindAsync(livroDto.GeneroLivroId)
            };

            if (livro == null)
            {
                return BadRequest();
            }

            _context.Livro.Add(livro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLivro", new { id = livro.livroId }, livro);
        }
        #endregion

        #region Métodos DELETE
        // DELETE: api/Livro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivro(int id)
        {
            var livro = await _context.Livro.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }

            _context.Livro.Remove(livro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
        private bool LivroExists(int id)
        {
            return _context.Livro.Any(e => e.livroId == id);
        }

    }
}
