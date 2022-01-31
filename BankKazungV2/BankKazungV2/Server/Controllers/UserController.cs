using BankKazungV2.Server.Data;
using BankKazungV2.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankKazungV2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == id);
            if(user == null)
            {
                return NotFound($"No User with UserID {id} was found");
            }
            return Ok(user);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(User user, int id)
        {
            var dbuser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == id);
            dbuser.FirstName = user.FirstName;
            dbuser.LastName = user.LastName;
            dbuser.Email = user.Email;
            dbuser.Age = user.Age;
            dbuser.Phone = user.Phone;
            
            _context.SaveChanges();

            return Ok(dbuser);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            var dbuser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == id);
            
            _context.Users.Remove(dbuser);

            _context.SaveChanges();

            return Ok("User Deleted");
        }
    }
}
