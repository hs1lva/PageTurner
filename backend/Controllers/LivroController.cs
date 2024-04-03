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
    public class LivroController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public LivroController(PageTurnerContext context)
        {
            _context = context;
        }

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

        // POST: api/Livro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Livro>> PostLivro(LivroCreateDTO livroDto)
        {
			var livro = new Livro { 
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

        private bool LivroExists(int id)
        {
            return _context.Livro.Any(e => e.livroId == id);
        }
    }
}
