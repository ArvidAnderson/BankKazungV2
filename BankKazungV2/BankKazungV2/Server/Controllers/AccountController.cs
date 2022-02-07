using BankKazungV2.Server.Data;
using BankKazungV2.Server.JwtHelp;
using BankKazungV2.Shared.DataTransferObjects;
using BankKazungV2.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankKazungV2.Server.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Account>> GetAccountByJWT(int Id)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            if(Token == null) { return BadRequest("No Access Token Provided"); }

            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountID == Id);

            if(account == null) { return NotFound("Account Not Found"); }
            if(account.UserID != UserID) { return BadRequest("Account Not Owned By You"); }

            return Ok(account);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAccountsByJWT()
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            if(Token == null) { return BadRequest("No Access Token Provided"); }

            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            List<Account> UserAccounts = await _context.Accounts.Where(u => u.UserID == UserID).ToListAsync();

            return Ok(UserAccounts);
        }

        [HttpPost("add/{Name}")]
        public async Task<ActionResult<Account>> AddAccountByJWT(string Name)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            if (Token == null) { return BadRequest("No Access Token Provided"); }

            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            Account newAccount = new Account();
            newAccount.Name = Name;
            newAccount.UserID = UserID;

            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            return Ok(newAccount);
        }

        [HttpDelete("remove/{Id}")]
        public async Task<IActionResult> DeleteAccountByJWT(int Id)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            if (Token == null) { return BadRequest("No Access Token Provided"); }

            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var accountToDelete = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountID == Id);

            if(accountToDelete == null) { return BadRequest("Account Not Found"); }
            if(accountToDelete.UserID != UserID) { return BadRequest("Account Not Owned By You"); }

            _context.Accounts.Remove(accountToDelete);
            _context.SaveChanges();

            return Ok($"Deleted Account: {Id}");
        }
    }
}
