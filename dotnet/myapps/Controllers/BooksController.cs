using libreria.WebApi.DTOs;
using libreria.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace libreria.WebApi.Controllers
{
    [Route("api/users/{userId:int}/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BooksController(AppDbContext context) => _context = context;

        // GET: api/users/{userId}/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(int userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                return NotFound(new { message = $"No se encontró el usuario con id {userId}" });

            return await _context.Books.Where(b => b.UserId == userId).ToListAsync();
        }

        // GET: api/users/{userId}/Books/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBook(int userId, int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (book == null)
                return NotFound(new { message = $"No se encontró el libro con id {id} para el usuario {userId}" });

            return book;
        }

        // POST: api/users/{userId}/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(int userId, BookCreateDto dto)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                return NotFound(new { message = $"No se encontró el usuario con id {userId}" });

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                PublishYear = dto.PublishYear,
                CoverUrl = dto.CoverUrl,
                UserId = userId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { userId, id = book.Id }, book);
        }

        // PUT: api/users/{userId}/Books/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBook(int userId, int id, BookUpdateDto dto)
        {
            // Validar existencia del libro
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (book == null)
                return NotFound(new { message = $"No se encontró el libro con id {id} para el usuario {userId}" });

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Author))
                return BadRequest(new { message = "El título y el autor son obligatorios" });

            if (dto.PublishYear <= 0)
                return BadRequest(new { message = "El año de publicación debe ser mayor que cero" });

            // Validar rating si viene
            if (dto.Rating.HasValue && (dto.Rating < 1 || dto.Rating > 5))
                return BadRequest(new { message = "El rating debe estar entre 1 y 5" });

            // Actualizar propiedades
            book.Title = dto.Title;
            book.Author = dto.Author;
            book.PublishYear = dto.PublishYear;
            book.CoverUrl = dto.CoverUrl;
            book.Rating = dto.Rating;
            book.Comment = dto.Comment;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}/review")]
        public async Task<IActionResult> PutReview(int userId, int id, [FromBody] ReviewDto dto)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (book == null)
                return NotFound(new { message = $"No se encontró el libro con id {id} para el usuario {userId}" });

            // Validar rating
            if (dto.Rating < 1 || dto.Rating > 5)
                return BadRequest(new { message = "El rating debe estar entre 1 y 5" });

            // Actualizar solo reseña
            book.Rating = dto.Rating;
            book.Comment = dto.Comment;

            await _context.SaveChangesAsync();
            return NoContent();
        }



        // DELETE: api/users/{userId}/Books/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int userId, int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (book == null)
                return NotFound(new { message = $"No se encontró el libro con id {id} para el usuario {userId}" });

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}