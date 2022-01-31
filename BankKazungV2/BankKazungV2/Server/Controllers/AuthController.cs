﻿using BankKazungV2.Server.Data;
using BankKazungV2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using BankKazungV2.Shared.DataTransferObjects;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BankKazungV2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegister request)
        {
            User user = new User();

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.Age = request.Age;
            user.Password = passwordHash;
            user.Salt = passwordSalt;

            if (user == null)
            {
                return BadRequest("Error Creating User");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok("Succesfully Created User");
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLogin _user)
        {
            User ?user = await _context.Users.SingleOrDefaultAsync(x => x.Email == _user.Email.ToLower());
            if(user == null)
            {
                return BadRequest("Wrong Email");
            }
            if (user != null && !VerifyPasswordHash(_user.Password, user.Password, user.Salt))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(user);
            return Ok(token);

        }

        [NonAction]
        private string CreateToken(User _user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.FirstName),
                new Claim(ClaimTypes.Surname, _user.LastName),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.NameIdentifier, _user.UserID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        } 

        [NonAction]
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [NonAction]
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
