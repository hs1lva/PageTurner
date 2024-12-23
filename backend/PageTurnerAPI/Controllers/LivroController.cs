using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        [HttpGet("{id}")]
public async Task<ActionResult> GetLivro(int id)
{
    var livro = await _context.Livro
        .Include(x => x.Comentarios)
        .Include(x => x.Avaliacoes)
        .Include(x => x.autorLivro)
        .Include(x => x.generoLivro)
        .FirstOrDefaultAsync(x => x.livroId == id);

    if (livro == null)
    {
        return NotFound();
    }

    var comentariosFiltrados = await _context.ComentarioLivro
        .Include(comentario => comentario.estadoComentario)
        .Where(comentario => comentario.estadoComentario.descricaoEstadoComentario == "Ativo" && comentario.livroId == id)
        .Select(c => new
        {
            c.comentarioId,
            c.comentario,
            c.dataComentario,
            c.utilizadorId,
            Utilizador = _context.Utilizador
                                .Where(u => u.utilizadorID == c.utilizadorId)
                                .Select(u => new
                                {
                                    u.nome,
                                    u.fotoPerfil
                                })
                                .FirstOrDefault()
        })
        .ToListAsync();

    var livroDto = new
    {
        LivroId = livro.livroId,
        TituloLivro = livro.tituloLivro,
        AnoPrimeiraPublicacao = livro.anoPrimeiraPublicacao,
        AutorLivro = livro.autorLivro,
        GeneroLivro = livro.generoLivro,
        MediaAvaliacao = livro.MediaAvaliacao(),
        Comentarios = comentariosFiltrados,
        Avaliacoes = livro.Avaliacoes,
        CapaSmall = livro.capaSmall,
        CapaMedium = livro.capaMedium,
        CapaLarge = livro.capaLarge
    };

    return Ok(livroDto);
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

        /// <summary>
        /// Método para pesquisar um livro por qualquer termo na Open Library
        /// </summary>
        /// <param name="termo"></param>
        /// <returns></returns>
        // GET: api/Livro/Pesquisar/OpenLibrary/{termo}
        [HttpGet("PesquisarLivro/OpenLibrary/{termo}")]
        public async Task<string> PesquisarLivroOL(string termo)
        {
            var servicoAPI = new Services.ServicoAPI();
            var livros = await servicoAPI.BuscarLivrosOpenLibrary("q", termo);

            if (livros == null)
            {
                return NotFound().ToString();
            }

            return livros;
        }

        /// <summary>
        /// Método para pesquisar um livro pelo título na Open Library
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns></returns>
        // GET: api/Livro/Pesquisar/OpenLibrary/{titulo}
        [HttpGet("PesquisarLivro/OpenLibrary/Titulo/{titulo}")]
        public async Task<string> PesquisarLivroTituloOL(string titulo)
        {
            var servicoAPI = new Services.ServicoAPI();
            var livros = await servicoAPI.BuscarLivrosOpenLibrary("title", titulo);

            if (livros == null)
            {
                return NotFound().ToString();
            }

            return livros;
        }

        /// <summary>
        /// Método para pesquisar um livro pelo autor na Open Library
        /// </summary>
        /// <param name="autor"></param>
        /// <returns></returns>
        // GET: api/Livro/Pesquisar/OpenLibrary/{autor}
        [HttpGet("PesquisarLivro/OpenLibrary/Autor/{autor}")]
        public async Task<string> PesquisarLivroAutorOL(string autor)
        {
            var servicoAPI = new Services.ServicoAPI();
            var livros = await servicoAPI.BuscarLivrosOpenLibrary("author", autor);

            if (livros == null)
            {
                return NotFound().ToString();
            }

            return livros;
        }

        /// <summary>
        /// Método para pesquisar um livro pelo género na Open Library
        /// </summary>
        /// <param name="genero"></param>
        /// <returns></returns>
        // GET: api/Livro/Pesquisar/OpenLibrary/{genero}
        [HttpGet("PesquisarLivro/OpenLibrary/Genero/{genero}")]
        public async Task<string> PesquisarLivroGeneroOL(string genero)
        {
            var servicoAPI = new Services.ServicoAPI();
            var livros = await servicoAPI.BuscarLivrosOpenLibrary("subject", genero);

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
        [HttpPost("PostLivroOL")]
        public async Task<ActionResult> PostLivroOL(LivroOLDTO livroDto)
        {
            try
            {
                // Verificar se o livro já existe na base de dados
                var existingBook = await _context.Livro.FirstOrDefaultAsync(x => x.keyOL == livroDto.KeyOL);
                if (existingBook != null)
                {
                    return Ok(new { LivroId = existingBook.livroId });
                }

                // Verificar se o nome do autor está disponível no DTO
                var autorNome = livroDto.AutorLivroNome.FirstOrDefault();
                AutorLivro autor = null;
                if (!string.IsNullOrEmpty(autorNome))
                {
                    autor = await _context.AutorLivro.FirstOrDefaultAsync(a => a.nomeAutorNome == autorNome);
                    if (autor == null)
                    {
                        autor = new AutorLivro
                        {
                            nomeAutorNome = autorNome
                        };
                        _context.AutorLivro.Add(autor);
                        await _context.SaveChangesAsync();
                    }
                }

                // Verificar se o nome do gênero está disponível no DTO
                var generoNome = livroDto.GeneroLivroNome.FirstOrDefault() ?? "Geral"; // Usar "Geral" se for nulo ou vazio
                var genero = await _context.GeneroLivro.FirstOrDefaultAsync(g => g.descricaoGenero == generoNome);
                if (genero == null)
                {
                    genero = new GeneroLivro
                    {
                        descricaoGenero = generoNome
                    };
                    _context.GeneroLivro.Add(genero);
                    await _context.SaveChangesAsync();
                }

                // Criar o novo livro com os dados fornecidos
                var livro = new Livro
                {
                    tituloLivro = livroDto.TituloLivro,
                    anoPrimeiraPublicacao = livroDto.AnoPrimeiraPublicacao,
                    keyOL = livroDto.KeyOL,
                    capaSmall = livroDto.CapaSmall ?? string.Empty,
                    capaMedium = livroDto.CapaMedium ?? string.Empty,
                    capaLarge = livroDto.CapaLarge ?? string.Empty,
                    autorLivro = autor,
                    generoLivro = genero
                };

                _context.Livro.Add(livro);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetLivro", new { id = livro.livroId }, new { LivroId = livro.livroId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro inesperado: {ex.Message}");
            }
        }


        // POST: api/Livro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostLivro")]
        public async Task<ActionResult<Livro>> PostLivro(LivroCreateDTO livroDto)
        {
            var livro = new Livro
            {
                tituloLivro = livroDto.TituloLivro,
                anoPrimeiraPublicacao = livroDto.AnoPrimeiraPublicacao,
                //idiomaOriginalLivro = livroDto.IdiomaOriginalLivro,
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

        [HttpGet("Top5Livros")]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetTop5Livros()
        {
            var livros = await _context.Livro
                .Include(l => l.autorLivro)
                .Include(l => l.generoLivro)
                .Include(l => l.Avaliacoes)
                .ToListAsync();

            var top5Livros = livros
                .OrderByDescending(l => l.MediaAvaliacao())
                .Take(5)
                .Select(l => new LivroDTO
                {
                    LivroId = l.livroId,
                    TituloLivro = l.tituloLivro,
                    AnoPrimeiraPublicacao = l.anoPrimeiraPublicacao,
                    AutorLivro = l.autorLivro,
                    GeneroLivro = l.generoLivro,
                    MediaAvaliacao = l.MediaAvaliacao(),
                    CapaSmall = l.capaSmall,
                    CapaMedium = l.capaMedium,
                    CapaLarge = l.capaLarge
                })
                .ToList();

            return Ok(top5Livros);
        }

    }
}
