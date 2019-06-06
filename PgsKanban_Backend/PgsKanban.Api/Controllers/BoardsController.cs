using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PgsKanban.Api.Extensions;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.BusinessLogic.Responses;
using PgsKanban.Dto;

namespace PgsKanban.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class BoardsController : Controller
    {
        private readonly IBoardService _boardService;
        private readonly ILogger<BoardsController> _logger;

        public BoardsController(IBoardService boardService, ILogger<BoardsController> logger)
        {
            _boardService = boardService;
            _logger = logger;
        }

        [HttpGet, Route("{id:int}", Name = "GetBoard"),]
        public IActionResult GetBoard(int id)
        {
            var userId = User.GetUserId();
            var board = _boardService.GetBoard(id, userId);

            return HandleGettingBoard(id.ToString(), board, userId);
        }

        [HttpGet, Route("getBoardByObfuscatedId/{obfuscatedId}")]
        public IActionResult GetBoardByObfuscatedId(string obfuscatedId)
        {
            var userId = User.GetUserId();
            var board = _boardService.GetBoardByObfuscatedId(obfuscatedId, userId);

            return HandleGettingBoard(obfuscatedId, board, userId);
        }

        [HttpGet, Route("getBoardByCardId/{cardObfuscatedId}")]
        public IActionResult GetBoardByCardId(string cardObfuscatedId)
        {
            var userId = User.GetUserId();
            var board = _boardService.GetBoardByCardId(cardObfuscatedId, userId);

            return HandleGettingBoardByCardId(cardObfuscatedId, board, userId);
        }

        private IActionResult HandleGettingBoard(string identifier, QueryResponse<BoardDto> board, string userId)
        {
            if (board.HttpStatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogInformation($"User with id: {userId} try to find board with identifier: {identifier} which doesn't exists");
                return NotFound();
            }

            if (board.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                _logger.LogInformation($"User with id: {userId} try to get board with identifier: {identifier} without permission");
                return Forbid();
            }
            _logger.LogInformation($"User with id: {userId} just get access board with identifier: {identifier}");
            return Ok(board.ResponseDto);
        }

        private IActionResult HandleGettingBoardByCardId(string cardId, QueryResponse<BoardDto> board, string userId)
        {
            if (board.HttpStatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogInformation($"User with id: {userId} try to find board by card with identifier: {cardId} which doesn't exists");
                return NotFound();
            }

            if (board.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                _logger.LogInformation($"User with id: {userId} try to get board by card with identifier: {cardId} without permission");
                return Forbid();
            }
            _logger.LogInformation($"User with id: {userId} just get access board by card with identifier: {cardId}");
            return Ok(board.ResponseDto);
        }

        [HttpGet, Route("exists")]
        public IActionResult HasUserBoards()
        {
            var hasUserBoards = _boardService.HasUserBoards(User.GetUserId());
            return Ok(hasUserBoards);
        }

        [HttpPost, Route("init")]
        public IActionResult CreateFirstBoard([FromBody] FirstBoardDto boardDto)
        {
            var userId = User.GetUserId();
            var addedBoard = _boardService.CreateFirstBoard(boardDto, userId);
            addedBoard.IsOwner = true;
            return CreatedAtRoute("GetBoard", new { id = addedBoard.BoardId }, addedBoard);
        }

        [HttpPost, Route("")]
        public IActionResult CreateBoard([FromBody] AddBoardDto boardDto)
        {
            var userId = User.GetUserId();
            var addedBoard = _boardService.CreateBoard(boardDto, userId);
            addedBoard.IsOwner = true;
            return CreatedAtRoute("GetBoard", new { id = addedBoard.BoardId }, addedBoard);
        }

        [HttpGet, Route("")]
        public IActionResult GetBoardsOfUser()
        {
            var userId = User.GetUserId();
            var boards = _boardService.GetBoardsOfUser(userId);
            return Ok(boards);
        }

        [HttpPost, Route("favorite/{boardId}")]
        public IActionResult ChangeFavoriteState(int boardId)
        {
            var userId = User.GetUserId();
            var result = _boardService.ChangeFavoriteState(boardId, userId);
            return result == null ? (IActionResult)BadRequest() : Ok(result.IsFavorite);
        }

        [HttpPut, Route("")]
        public IActionResult EditBoard([FromBody] EditBoardDto boardDto)
        {
            var userId = User.GetUserId();
            var editedBoard = _boardService.EditBoard(boardDto, userId);
            if (editedBoard == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete, Route("delete/{boardId}")]
        public IActionResult DeleteBoard(int boardId)
        {
            var userId = User.GetUserId();
            var deletedBoard = _boardService.DeleteBoard(boardId, userId);
            if (deletedBoard == null)
            {
                return NotFound();
            }
            return Ok(deletedBoard.Id);
        }

        [HttpPost, Route("member")]
        public async Task<IActionResult> AddMember([FromBody] AddMemberDto addMemberDto)
        {
            var userId = User.GetUserId();
            var addedMember = await _boardService.ConnectUserWithBoard(addMemberDto, userId);
            if (addedMember == null)
            {
                return BadRequest();
            }
            _logger.LogInformation($"User with id: {userId} added member with id {addMemberDto.MemberId} to the board with id: {addMemberDto.BoardId}");
            return Ok(addedMember);
        }

        [HttpDelete, Route("member")]
        public IActionResult DeleteMember([FromBody] DeleteMemberDto deleteMemberDto)
        {
            var userId = User.GetUserId();
            var deletedUser = _boardService.DeleteMember(deleteMemberDto.BoardId, deleteMemberDto.MemberId, userId);
            if (deletedUser == null)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpGet, Route("member/{boardId}")]
        public IActionResult GetMembers(int boardId)
        {
            var userId = User.GetUserId();
            var members = _boardService.GetMembersOfBoard(boardId, userId);
            return Ok(members);
        }
    }
}
