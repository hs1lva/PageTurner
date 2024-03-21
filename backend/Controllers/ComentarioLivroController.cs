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
    public class ComentarioLivroController : ControllerBase
    {
        private readonly PageTurnerContext _context;

        public ComentarioLivroController(PageTurnerContext context)
        {
            _context = context;
        }

        // GET: api/ComentarioLivro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComentarioLivro>>> GetCommentLivro()
        {
            return await _context.CommentLivro.ToListAsync();
        }

        // GET: api/ComentarioLivro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComentarioLivro>> GetComentarioLivro(int id)
        {
            var comentarioLivro = await _context.CommentLivro.FindAsync(id);

            if (comentarioLivro == null)
            {
                return NotFound();
            }

            return comentarioLivro;
        }

        // PUT: api/ComentarioLivro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComentarioLivro(int id, ComentarioLivro comentarioLivro)
        {
            if (id != comentarioLivro.comentarioId)
            {
                return BadRequest();
            }

            _context.Entry(comentarioLivro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioLivroExists(id))
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

        // POST: api/ComentarioLivro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ComentarioLivro>> PostComentarioLivro(ComentarioLivro comentarioLivro)
        {
            _context.CommentLivro.Add(comentarioLivro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComentarioLivro", new { id = comentarioLivro.comentarioId }, comentarioLivro);
        }

        // DELETE: api/ComentarioLivro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentarioLivro(int id)
        {
            var comentarioLivro = await _context.CommentLivro.FindAsync(id);
            if (comentarioLivro == null)
            {
                return NotFound();
            }

            _context.CommentLivro.Remove(comentarioLivro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComentarioLivroExists(int id)
        {
            return _context.CommentLivro.Any(e => e.comentarioId == id);
        }
    }
}
