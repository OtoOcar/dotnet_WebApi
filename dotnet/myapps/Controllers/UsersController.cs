using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using libreria.WebApi.Models;
using libreria.WebApi.DTOs.libreria.WebApi.DTOs;

namespace libreria.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewDto>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UserViewDto(u.Id, u.Name, u.LastName, u.Email))
                .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewDto>> GetUsers(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = $"No se encontró el usuario con id {id}" });

            return new UserViewDto(user.Id, user.Name, user.LastName, user.Email);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserViewDto>> PostUsers(UserRegisterDto dto)
        {
            // Validar email único
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict("El email ya está registrado");

            var user = new Users
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password // aquí podrías aplicar hashing si quieres
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id },
                new UserViewDto(user.Id, user.Name, user.LastName, user.Email));
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, UserRegisterDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = $"No se encontró el usuario con id {id}" });

            // Validación de email duplicado
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id);
            if (emailExists)
                return Conflict(new { message = "El email ya está registrado por otro usuario" });

            user.Name = dto.Name;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Password = dto.Password;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = $"No se encontró el usuario con id {id}" });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}