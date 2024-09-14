using FixitAPI.Data;
using FixitAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FixitAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User details are required.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User saved successfully.");
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (updatedUser == null)
            {
                return BadRequest("User details are required.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Age = updatedUser.Age;
            existingUser.Gender = updatedUser.Gender;
            existingUser.City = updatedUser.City;
            existingUser.State = updatedUser.State;
            existingUser.Contact = updatedUser.Contact;
            await _context.SaveChangesAsync();

            return Ok("User updated successfully.");
        }

    }
}
