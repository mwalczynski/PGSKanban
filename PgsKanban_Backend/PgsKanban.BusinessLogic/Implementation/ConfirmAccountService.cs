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
    public class ConfirmAccountService: IConfirmAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IMessageSender _messageSender;
        private readonly IConfigurationRoot _configuration;

        public ConfirmAccountService(UserManager<User> userManager, IUserStore<User> userStore, IMessageSender messageSender, IConfigurationRoot configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _messageSender = messageSender;
            _configuration = configuration;
        }

        public async Task<string> SendEmailConfirmationToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.SaveEmailConfirmationToken(_userStore, user, _configuration);

            var baseUrl = _configuration["ApplicationUrl:localhost"];
            var callbackUrl = $"{baseUrl}/login/{token}";
            _messageSender.SendEmail("Confirm Account", _messageSender.GetActivationTemplateId(), user, callbackUrl);

            return token;
        }

        public async Task<ConfirmationEmailResponseDto> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.EmailConfirmationToken == confirmEmailDto.ConfirmationToken);
            var confirmationEmailResponse = await _userManager.ConfirmEmail(_userStore, user);
            return confirmationEmailResponse;
        }

        public async Task<string> SendEmailConfirmationEmailByToken(string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.EmailConfirmationToken == token);
            if (user == null || user.EmailConfirmed)
            {
                return null;
            }
            return await SendEmailConfirmationToken(user.Email);
        }
    }
}
