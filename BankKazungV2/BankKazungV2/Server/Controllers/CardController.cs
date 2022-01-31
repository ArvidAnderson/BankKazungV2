using BankKazungV2.Server.Data;
using BankKazungV2.Server.JwtHelp;
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
    public class CardController : ControllerBase
    {
        private readonly DataContext _context;

        public CardController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("jwt/get/cards")]
        public async Task<IActionResult> GetAccountsByJWT()
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            List<Card> UserCards = await _context.Cards.Where(u => u.UserID == UserID).ToListAsync();

            return Ok(UserCards);
        }

        [HttpGet("jwt/get/card")]
        public async Task<ActionResult<Card>> GetAccountByJWT(int _cardID)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var card = await _context.Cards.SingleOrDefaultAsync(c => c.CardID == _cardID);

            if (card == null)
            {
                return NotFound("Card Not Found");
            }

            if (card.UserID == UserID)
            {
                return Ok(card);
            }

            return BadRequest("Error Getting Card: !Permission");
        }

        [HttpPost("jwt/add/card")]
        public async Task<ActionResult<Card>> AddAccountByJWT(string _name, CardTypes _type)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            Card newCard = new Card();
            newCard.Name = _name;
            newCard.UserID = UserID;
            newCard.Type = _type;
            if(_type == CardTypes.Credit)
            {
                newCard.CreditLimit = 1000;
            }

            _context.Cards.Add(newCard);
            _context.SaveChanges();

            return Ok(newCard);
        }

        [HttpDelete("jwt/remove/card")]
        public async Task<IActionResult> DeleteAccountByJWT(int _cardID)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var cardToDelete = await _context.Cards.SingleOrDefaultAsync(c => c.CardID == _cardID);

            if (cardToDelete == null)
            {
                return NotFound("Card Not Found");
            }

            if (cardToDelete.UserID == UserID)
            {
                _context.Cards.Remove(cardToDelete);
                _context.SaveChanges();
                return Ok($"Deleted Account: {_cardID}");
            }

            return BadRequest("Error Deleting Card: !Permission");
        }
    }
}
