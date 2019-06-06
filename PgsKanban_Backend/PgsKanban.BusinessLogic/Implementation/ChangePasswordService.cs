using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PgsKanban.BusinessLogic.Extensions.UserManagerExtensions;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class ChangePasswordService: IChangePasswordService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IMessageSender _messageSender;
        private readonly IConfigurationRoot _configuration;

        public ChangePasswordService(UserManager<User> userManager, IUserStore<User> userStore, IMessageSender messageSender, IConfigurationRoot configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _messageSender = messageSender;
            _configuration = configuration;
        }

        public async Task<string> SendResetLink(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return null;
            }
            var token = await _userManager.SavePasswordResetToken(_userStore, user, int.Parse(_configuration["Confirmation:ResetPasswordTokenLifeSpanInHours"]));
            if (token != null)
            {
                var url = _configuration["ApplicationUrl:localhost"];
                var callbackUrl = $"{url}/reset/{token}";
                _messageSender.SendEmail("Reset password", _messageSender.GetResetPasswordTemplateId(), user, callbackUrl);
            }
            return token;
        }

        public async Task SendPasswordChangeInformation(string userEmail, string userBrowser, string userBrowserVersion)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                _messageSender.SendEmail("Password change", _messageSender.GetChangePasswordTemplateId(), user, userBrowser, userBrowserVersion); ;
            }
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user =
                await FindUserByToken(resetPasswordDto.Token);
            if (user == null)
            {
                return null;
            }
            var resetResult = await _userManager.ResetPassword(_userStore, user, resetPasswordDto.Password);

            return resetResult;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordUserDto changePasswordUserDto, string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return null;
            }

            var changeResult = await _userManager.ChangePasswordAsync(user, changePasswordUserDto.OldPassword,
                changePasswordUserDto.NewPassword);

            return changeResult;
        }

        public async Task<ValidatePasswordResetTokenResponseDto> ValidateToken(string token)
        {
            var response = new ValidatePasswordResetTokenResponseDto();
            var user = await FindUserByToken(token);
            if (user == null)
            {
                response.Invalid = true;
                return response;
            }
            if (user.PasswordResetTokenExpirationTime < DateTime.UtcNow)
            {
                response.Expired = true;
            }
            return response;
        }

        public async Task<IdentityResult> AddPassword(AddPasswordDto addPasswordDto, string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var hasUserPassword = await _userManager.HasPasswordAsync(user);

            if (user == null || hasUserPassword)
            {
                return null;
            }

            var addResult = await _userManager.AddPasswordAsync(user, addPasswordDto.NewPassword);

            return addResult;
        }

        private async Task<User> FindUserByToken(string token)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == token);
        }
    }
}
