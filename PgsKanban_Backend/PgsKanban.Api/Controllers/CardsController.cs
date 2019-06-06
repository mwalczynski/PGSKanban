using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PgsKanban.Api.Extensions;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.Dto;
using PgsKanban.Hubs.Hubs;
using PgsKanban.Hubs.Interfaces;

namespace PgsKanban.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class CardsController : Controller
    {
        private readonly ICardService _cardService;
        private readonly IHubContext<CardDetailsHub, ICardDetailsClientHandler> _cardDetailsHub;

        public CardsController(ICardService cardService, IHubContext<CardDetailsHub, ICardDetailsClientHandler> cardDetailsHub)
        {
            _cardService = cardService;
            _cardDetailsHub = cardDetailsHub;
        }

        [HttpGet, Route("details/{obfuscatedId}", Name = "GetCard")]
        public IActionResult GetCardDetails(string obfuscatedId)
        {
            var userId = User.GetUserId();
            var card = _cardService.GetCardDetails(obfuscatedId, userId);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        [HttpPost, Route("")]
        public IActionResult CreateCard([FromBody] AddCardDto cardDto)
        {
            var userId = User.GetUserId();
            var addedCard = _cardService.CreateCard(cardDto, userId);

            if (addedCard == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetCard", new
            {
                obfuscatedId = addedCard.ObfuscatedId
            }, addedCard);
        }

        [HttpPost, Route("comment")]
        public IActionResult CreateComment([FromBody] CommentCardDto commentDto)
        {
            var userId = User.GetUserId();
            var addedComment = _cardService.AddCommentToCard(commentDto, userId);

            if (addedComment == null)
            {
                return BadRequest();
            }

            _cardDetailsHub.Clients.Group(commentDto.CardId.ToString()).AddComment(addedComment);
            return Ok(addedComment);
        }

        [HttpPut, Route("")]
        public IActionResult EditCardName([FromBody] EditCardNameDto cardDto)
        {
            var userId = User.GetUserId();
            var editedCard = _cardService.EditCard(cardDto, userId);
            if (editedCard == null)
            {
                return NotFound();
            }

            _cardDetailsHub.Clients.Group(cardDto.Id.ToString()).ChangedName(cardDto.Name);

            return NoContent();
        }

        [HttpPut, Route("description")]
        public IActionResult EditCardDescription([FromBody] EditCardDescriptionDto cardDto)
        {
            var userId = User.GetUserId();
            var editedCard = _cardService.EditCardDescription(cardDto, userId);
            if (editedCard == null)
            {
                return NotFound();
            }

            _cardDetailsHub.Clients.Group(cardDto.Id.ToString()).ChangedLongDescription(cardDto.Description);

            return Ok(editedCard);
        }

        [HttpPut, Route("move")]
        public IActionResult EditCardPosition([FromBody] EditCardPositionDto cardDto)
        {
            var userId = User.GetUserId();
            var editedCard = _cardService.EditCardPosition(cardDto, userId);

            if (editedCard == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete, Route("")]
        public IActionResult DeleteCard([FromBody] DeleteCardDto deleteCardDto)
        {
            var userId = User.GetUserId();

            var deletedCard = _cardService.DeleteCard(deleteCardDto, userId);

            if(deletedCard == null)
            {
                return NotFound();
            }

            return Ok(deletedCard.Id);
        }

    }
}
