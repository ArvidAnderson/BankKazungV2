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
    [Route("api/card")]
    [ApiController]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly DataContext _context;

        public CardController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Card>> GetCardByJWT(int Id)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            if (Token == null) { return BadRequest("No Access Token Provided"); }

            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var card = await _context.Cards.SingleOrDefaultAsync(c => c.CardID == Id);

            if (card == null) { return NotFound("Card Not Found"); }
            if (card.UserID != UserID) { return BadRequest("Card Not Owned By You"); }

            return Ok(card);
        }

        [HttpGet("all")]
        public async Task<ActionResult<Card[]>> GetCardsByJWT()
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            if (Token == null) { return BadRequest("No Access Token Provided"); }

            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            List<Card> UserCards = await _context.Cards.Where(u => u.UserID == UserID).ToListAsync();

            if (UserCards.Count == 0) { return BadRequest("Cards Not Found"); }

            return Ok(UserCards);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Card>> AddCardByJWT(CardAdd _newCard)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            if (Token == null) { return BadRequest("No Access Token Provided"); }

            if(_newCard.Type != CardTypes.Credit && _newCard.Type != CardTypes.Debit) { return BadRequest("Card Type Invalid"); }

            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            Card newCard = new Card();
            newCard.Name = _newCard.Name;
            newCard.UserID = UserID;
            newCard.Type = _newCard.Type;
            if(_newCard.Type == CardTypes.Credit)
            {
                newCard.CreditLimit = 1000;
            }

            _context.Cards.Add(newCard);
            _context.SaveChanges();

            return Ok(newCard);
        }

        [HttpDelete("remove/{Id}")]
        public async Task<IActionResult> DeleteCardByJWT(int Id)
        {
            var Token = await HttpContext.GetTokenAsync("access_token");
            if (Token == null) { return BadRequest("No Access Token Provided"); }

            var UserID = JwtHelper.DecodeUserIDFromToken(Token);

            var cardToDelete = await _context.Cards.SingleOrDefaultAsync(c => c.CardID == Id);

            if (cardToDelete == null) { return NotFound("Card Not Found"); }
            if (cardToDelete.UserID != UserID) { return BadRequest("Card Not Owned By You"); }

            _context.Cards.Remove(cardToDelete);
            _context.SaveChanges();
            return Ok($"Deleted Account: {Id}");
        }
    }
}
