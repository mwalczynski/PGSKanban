using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PgsKanban.Api.Extensions;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.Dto;

namespace PgsKanban.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class ListsController : Controller
    {
        private readonly IListService _listService;

        public ListsController(IListService listService)
        {
            _listService = listService;
        }

        [HttpGet, Route("", Name = "GetList")]
        public IActionResult GetList([FromQuery] int id)
        {
            var userId = User.GetUserId();
            var list = _listService.GetList(id, userId);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpPost, Route("")]
        public IActionResult CreateList([FromBody] AddListDto listDto)
        {
            var userId = User.GetUserId();
            var addedList = _listService.CreateList(listDto, userId);
            if (addedList == null)
            {
                return BadRequest();
            }
            return CreatedAtRoute("GetList", new
            {
                id = addedList.Id
            }, addedList);
        }

        [HttpPut, Route("")]
        public IActionResult EditList([FromBody] EditListDto listDto)
        {
            var userId = User.GetUserId();
            var editedList = _listService.EditList(listDto, userId);
            if (editedList == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteList([FromBody] DeleteListDto deleteListDto)
        {
            var userId = User.GetUserId();
            var deletedList = _listService.DeleteList(deleteListDto, userId);
            if (deletedList == null)
            {
                return NotFound();
            }
            return Ok(deletedList.Id);
        }

        [HttpPut, Route("move")]
        public IActionResult EditListPosition([FromBody] EditListPositionDto listDto)
        {
            var userId = User.GetUserId();
            var editedList = _listService.EditListPosition(listDto, userId);
            if (editedList == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
