using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PgsKanban.Api.Extensions;
using PgsKanban.BusinessLogic.Enums;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.Dto;

namespace PgsKanban.Api.Controllers
{
    [Route("/api/external")]
    public class ExternalLoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<ExternalLoginController> _logger;
        private readonly IExternalLoginService _externalLoginService;

        public ExternalLoginController(IAuthService authService, IExternalLoginService externalLoginService, ILogger<ExternalLoginController> logger)
        {
            _authService = authService;
            _externalLoginService = externalLoginService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult ExternalLoginOpenId()
        {
            var userIp = Request.HttpContext.Connection.RemoteIpAddress;
            
            var redirectUri = _externalLoginService.GetOpenIdUrl(userIp);
            _logger.LogInformation($"User with ip: {userIp} started openId logging flow");
            return Ok(redirectUri);
        }

        [HttpGet, Route("google")]
        public IActionResult ExternalLoginGoogleAccount()
        {
            var userIp = Request.HttpContext.Connection.RemoteIpAddress;

            var redirectUri = _externalLoginService.GetGoogleUrl(userIp);
            _logger.LogInformation($"User with ip: {userIp} started google logging flow");
            return Ok(redirectUri);
        }

        [HttpPost, Route("facebook")]
        public async Task<IActionResult> ExternalLoginFacebook([FromBody] FacebookLoginDto loginInfo)
        {
            var userIp = Request.HttpContext.Connection.RemoteIpAddress;
            var externalData = await _externalLoginService.GetExternalFacebookLoginData(loginInfo.AccessToken);
            return await HandleExternalInfo(userIp, externalData, ExternalLoginProvider.Facebook);
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginInfoDto externalLoginInfoDto)
        {
            var userIp = Request.HttpContext.Connection.RemoteIpAddress;
            var externalData = await _externalLoginService.GetExternalLoginData(externalLoginInfoDto, userIp);
            return await HandleExternalInfo(userIp, externalData, ExternalLoginProvider.OpenId);
        }

        private async Task<IActionResult> HandleExternalInfo(IPAddress userIp, ExternalLoginDataDto externalData, ExternalLoginProvider loginProvider)
        {
            _logger.LogInformation($"User with ip: {userIp} started facebook logging flow");
            if (externalData == null)
            {
                _logger.LogInformation(
                    $"User with ip: {userIp} tried to login with external login provider. Unable to validate external token");
                return BadRequest();
            }
            var result = await _authService.ExternalLogin(externalData, loginProvider);
            if (result == null)
            {
                _logger.LogInformation(
                    $"User with ip: {userIp} tried to login with external login provider. Unable to register user");
                return BadRequest();
            }
            return Ok(result.Token);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var externalLogoutData = User.GetExternalLogoutData();
            var result = _externalLoginService.Logout(externalLogoutData);
            return Ok(result);
        }
    }
}
