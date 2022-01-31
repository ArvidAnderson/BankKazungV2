using BankKazungV2.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BankKazungV2.Server.JwtHelp
{
    public static class JwtHelper
    {
        public static string CreateToken(User _user, byte[] _key)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("FirstName", _user.FirstName),
                new Claim("LastName", _user.LastName),
                new Claim("Email", _user.Email),
                new Claim("UserID", _user.UserID.ToString())
            };

            var key = new SymmetricSecurityKey(_key);
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static Dictionary<string, string> DecodeToken(string _token)
        {
            var TokenInfo = new Dictionary<string, string>();

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(_token);
            var claims = jwtSecurityToken.Claims.ToList();

            foreach (var claim in claims)
            {
                TokenInfo.Add(claim.Type, claim.Value);
            }

            return TokenInfo;
        }

        public static int DecodeUserIDFromToken(string _token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(_token);
            var claims = jwtSecurityToken.Claims.ToList();
            
            foreach(var claim in claims)
            {
                if(claim.Type == "UserID")
                {
                    return int.Parse(claim.Value);
                }
            }
            return 0;
        }
    }
}
