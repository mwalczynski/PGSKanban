using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PgsKanban.Api.Attributes;
using PgsKanban.Api.Extensions;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.BusinessLogic.Services.ReCaptcha.Interfaces;
using PgsKanban.Dto;

namespace PgsKanban.Api.Controllers
{
    [ServiceFilter(typeof(DisableIfNoLocalProviderAvailable))]
    [Route("/api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IChangePasswordService _changePasswordService;
        private readonly IConfirmAccountService _confirmAccountService;
        private readonly ILogger<AccountController> _logger;
        private readonly ICacheService _cacheService;
        private readonly IReCaptchaValidation _reCaptchaValidation;

        public AccountController(IAuthService authService, IChangePasswordService changePasswordService, IConfirmAccountService confirmAccountService,
                                ILogger<AccountController> logger, ICacheService cacheService, IReCaptchaValidation reCaptchaValidation)
        {
            _authService = authService;
            _changePasswordService = changePasswordService;
            _confirmAccountService = confirmAccountService;
            _logger = logger;
            _cacheService = cacheService;
            _reCaptchaValidation = reCaptchaValidation;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] LoginUserDto loginUserDto)
        {
            var userIp = Request.HttpContext.Connection.RemoteIpAddress;

            if (_cacheService.IsNumberOfAttempsExceeded(userIp.ToString()))
            {
                if (loginUserDto.RecaptchaResponse == null || !await _reCaptchaValidation.ValidateRecaptcha(loginUserDto.RecaptchaResponse))
                {
                    _logger.LogInformation($"Invalid captcha validation: {userIp}");
                    _cacheService.UpdateFailedAttempsCount(userIp.ToString());

                    return BadRequest(_reCaptchaValidation.CreateCaptchaLoginResponse(false, true, false, false));
                }
            }

            var tokenResponse = await _authService.Login(loginUserDto);

            bool resultOfHandlingLoginAttemps;

            if (tokenResponse == null)
            {
                resultOfHandlingLoginAttemps = _cacheService.HandleLoginAttemps(userIp);
                _logger.LogInformation($"Invalid login as user with email: {loginUserDto.Email}");
                return BadRequest(_reCaptchaValidation.CreateCaptchaLoginResponse(true, resultOfHandlingLoginAttemps, true, true));
            }

            if (tokenResponse.Token == null)
            {
                resultOfHandlingLoginAttemps = _cacheService.HandleLoginAttemps(userIp);
                if (!tokenResponse.IsAccountActive)
                {
                    _logger.LogInformation($"User with email: {loginUserDto.Email} has tried to log in with not activated account");
                    return BadRequest(_reCaptchaValidation.CreateCaptchaLoginResponse(true, resultOfHandlingLoginAttemps, false, false));
                }
            }

            _logger.LogInformation($"User with email: {loginUserDto.Email} just logged in");
            return Ok(tokenResponse.Token);
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> Register([FromBody]RegisterUserDto registerUserDto)
        {
            var registerResult = await _authService.Register(registerUserDto);

            if (registerResult.Succeeded)
            {
                var confirmationToken = await _confirmAccountService.SendEmailConfirmationToken(registerUserDto.Email);
                if (confirmationToken != null)
                {
                    _logger.LogInformation($"User with email: {registerUserDto.Email} just registered");
                    return Ok(registerResult);
                }
            }
            _logger.LogInformation($"Invalid register as user with email: {registerUserDto.Email}");
            return BadRequest(registerResult);
        }

        [AllowAnonymous]
        [HttpPost, Route("confirmation")]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailDto confirmEmailDto)
        {
            var result = await _confirmAccountService.ConfirmEmail(confirmEmailDto);
            if (!result.Succedeed)
            {
                _logger.LogInformation($"User with confirmation token: {confirmEmailDto.ConfirmationToken} has tried to activate account");
                return BadRequest(result);
            }
            _logger.LogInformation($"User with confirmation token: {confirmEmailDto.ConfirmationToken} has activated account");
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost, Route("resendConfirmation")]
        public async Task<IActionResult> ReSendConfirmationEmail([FromBody]ConfirmEmailDto confirmEmailDto)
        {
            var confirmationToken = await _confirmAccountService.SendEmailConfirmationEmailByToken(confirmEmailDto.ConfirmationToken);
            if (confirmationToken == null)
            {
                return BadRequest();
            }
            _logger.LogInformation($"User with confirmation token: {confirmEmailDto.ConfirmationToken} - resent email confirmation message");
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost, Route("resend")]
        public async Task<IActionResult> ReSendConfirmationEmail([FromBody]ResendEmailConfirmationDto confirmEmailDto)
        {
            var confirmationToken = await _confirmAccountService.SendEmailConfirmationToken(confirmEmailDto.Email);
            if (confirmationToken == null)
            {
                return BadRequest();
            }
            _logger.LogInformation($"User with email: {confirmEmailDto.Email} - resent email confirmation message");
            return NoContent();
        }

        [HttpGet, Route("captcha")]
        public IActionResult IsLoginAttempsCountExceeded()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var result = _cacheService.IsNumberOfAttempsExceeded(remoteIpAddress.ToString());
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost, Route("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody]RequestPasswordResetDto requestPasswordResetDto)
        {
            var userIp = Request.HttpContext.Connection.RemoteIpAddress;

            if (_cacheService.IsNumberOfAttempsExceededRecovery(userIp))
            {
                if (requestPasswordResetDto.RecaptchaResponse == null
                    || !await _reCaptchaValidation.ValidateRecaptcha(requestPasswordResetDto.RecaptchaResponse))
                {
                    _cacheService.UpdateAttempsCountRecovery(userIp);
                    _logger.LogInformation($"Invalid captcha validation: {userIp}");
                    return BadRequest(_reCaptchaValidation.CreateCaptchaResponseRecovery(false, true));
                }
            }
            var result = _cacheService.HandleAttempsRecovery(userIp);          
            var resetToken = await _changePasswordService.SendResetLink(requestPasswordResetDto.Email);

            _logger.LogInformation(resetToken == null
                ? $"User with email: {requestPasswordResetDto.Email} fails to generate token"
                : $"User with email: {requestPasswordResetDto.Email} send reset password request with token: {resetToken}"
            );
            return Ok(_reCaptchaValidation.CreateCaptchaResponseRecovery(true, result));
        }

        [HttpGet, Route("resetCaptcha")]
        public IActionResult IsResetPasswordAttempsCountExceeded()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var result = _cacheService.IsNumberOfAttempsExceededRecovery(remoteIpAddress);
            return Ok(result);
        }

        [HttpPost, Route("reset")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto resetPasswordDto)
        {
            var resetResult = await _changePasswordService.ResetPassword(resetPasswordDto);
            if (resetResult != null && resetResult.Succeeded)
            {
                _logger.LogInformation($"User with password reset token: {resetPasswordDto.Token} changed password");
                return NoContent();
            }
            _logger.LogInformation($"Invalid password reset as user with password reset token: {resetPasswordDto.Token}");
            return BadRequest();
        }

        [HttpPost, Route("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordUserDto changePasswordUserDto)
        {
            var userId = User.GetUserId();
            var userEmail = User.GetUserEmail();
            var userBrowser = changePasswordUserDto.Browser;
            var userBrowserVersion = changePasswordUserDto.BrowserVersion;

            var changePasswordResult = await _changePasswordService.ChangePassword(changePasswordUserDto, userId);            

            if (changePasswordResult != null && changePasswordResult.Succeeded)
            {
                await _changePasswordService.SendPasswordChangeInformation(userEmail, userBrowser, userBrowserVersion);
                _logger.LogInformation($"User with id: {userId} changed password");
                return NoContent();
            }
            _logger.LogInformation($"Ivalid password change as a user with id: {userId}");
            return BadRequest();
        }

        [HttpPost, Route("addPassword")]
        public async Task<IActionResult> AddPassword([FromBody] AddPasswordDto addPasswordDto)
        {
            var userId = User.GetUserId();

            var addPasswordResult = await _changePasswordService.AddPassword(addPasswordDto, userId);

            if (addPasswordResult != null && addPasswordResult.Succeeded)
            {
                _logger.LogInformation($"User with id: {userId} added password");
                return NoContent();
            }
            _logger.LogInformation($"Ivalid password adding as a user with id: {userId}");
            return BadRequest();
        }

        [HttpGet, Route("validate/{token}")]
        public async Task<IActionResult> ValidatePasswordResetToken(string token)
        {
            var result = await _changePasswordService.ValidateToken(token);

            return Ok(result);
        }
    }
}
