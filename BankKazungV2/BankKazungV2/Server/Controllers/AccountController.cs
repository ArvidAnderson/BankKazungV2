﻿using BankKazungV2.Server.Data;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("jwt/get/accounts")]
        public async Task<IActionResult> GetAccountsByJWT()
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            List<Account> UserAccounts = await _context.Accounts.Where(u => u.UserID == UserID).ToListAsync();

            return Ok(UserAccounts);
        }

        [HttpGet("jwt/get/account")]
        public async Task<ActionResult<Account>> GetAccountByJWT(int _accountID)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountID == _accountID);
            
            if(account == null)
            {
                return NotFound("Account Not Found");
            }

            if(account.UserID == UserID)
            {
                return Ok(account);
            }

            return BadRequest("Error Getting Account: !Permission");
        }

        [HttpPost("jwt/add/account")]
        public async Task<ActionResult<Account>> AddAccountByJWT(string _name)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            Account newAccount = new Account();
            newAccount.Name = _name;
            newAccount.UserID = UserID;

            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            return Ok(newAccount);
        }

        [HttpDelete("jwt/remove/account")]
        public async Task<IActionResult> DeleteAccountByJWT(int _accountID)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var accountToDelete = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountID == _accountID);

            if(accountToDelete == null)
            {
                return NotFound("Account Not Found");
            }

            if(accountToDelete.UserID == UserID)
            {
                _context.Accounts.Remove(accountToDelete);
                _context.SaveChanges();
                return Ok($"Deleted Account: {_accountID}");
            }

            return BadRequest("Error Deleting Account: !Permission");
        }
    }
}
