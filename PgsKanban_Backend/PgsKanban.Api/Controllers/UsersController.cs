using Microsoft.AspNetCore.Mvc;
using PgsKanban.BusinessLogic.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PgsKanban.Api.Attributes;
using PgsKanban.Api.Extensions;
using PgsKanban.Dto;

namespace PgsKanban.Api.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userDataService;

        public UsersController(IUserService userDataService)
        {
            _userDataService = userDataService;
        }

        [HttpGet, Route("profile")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var userEmail = User.GetUserEmail();
            var userData = await _userDataService.GetUserProfileByUserEmail(userEmail);
            return Ok(userData);
        }

        [HttpGet, Route("profile/secure")]
        public async Task<IActionResult> GetSecureUserProfile()
        {
            var userEmail = User.GetUserEmail();
            var userData = await _userDataService.GetSecureUserProfileByUserEmail(userEmail);
            return Ok(userData);
        }

        [HttpGet, Route("statistics")]
        public IActionResult GetUserStatistics()
        {
            var userId = User.GetUserId();
            var statistics = _userDataService.GetStatistics(userId);
            return Ok(statistics);
        }

        [HttpGet, Route("search")]
        public IActionResult GetSearchedUsers([FromQuery] SearchForUsersDto searchForUsersDto)
        {
            var userId = User.GetUserId();
            var foundUsers = _userDataService.FindUsers(searchForUsersDto, userId);
            return Ok(foundUsers);
        }

        [ServiceFilter(typeof(DisableIfNoLocalProviderAvailable))]
        [HttpPost, Route("edit")]
        public async Task<IActionResult> EditUserProfile([FromBody] EditUserProfileDto editUserProfileDto)
        {
            var userId = User.GetUserId();
            var result = await _userDataService.EditUserProfile(editUserProfileDto, userId);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost, Route("anonymous")]
        public async Task<IActionResult> EditAnonymousSettings([FromBody] EditUserAnonymousSettingsDto editUserAnonymousSettingsDto)
        {
            var userId = User.GetUserId();
            var result = await _userDataService.EditUserAnonymousSettingsProfile(editUserAnonymousSettingsDto, userId);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }
    }
}
