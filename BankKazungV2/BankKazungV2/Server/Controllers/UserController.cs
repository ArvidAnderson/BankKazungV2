using BankKazungV2.Server.Data;
using BankKazungV2.Server.JwtHelp;
using BankKazungV2.Shared.Models;
using Microsoft.AspNetCore.Authentication;
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

        [HttpGet("jwt/get/user"), Authorize]
        public async Task<ActionResult<User>> GetUserByJWT()
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserID == UserID);

            return Ok(user);
        }

        [HttpGet("jwt/get/user/full"), Authorize]
        public async Task<ActionResult<User>> GetFullUserByJWT()
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserID == UserID);
            if(user == null)
            {
                return NotFound("User Not Found!");
            }
            if(user != null)
            {
                user.Accounts = await _context.Accounts.Where(u => u.UserID == UserID).ToListAsync();
                user.Cards = await _context.Cards.Where(u => u.UserID == UserID).ToListAsync();
            }
            

            return Ok(user);
        }
    }
}
