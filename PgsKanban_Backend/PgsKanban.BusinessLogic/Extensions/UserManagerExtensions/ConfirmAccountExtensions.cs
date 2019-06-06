using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PgsKanban.BusinessLogic.Services;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Extensions.UserManagerExtensions
{
    public static class ConfirmAccountExtensions
    {
        public static async Task<string> SaveEmailConfirmationToken(this UserManager<User> userManager, IUserStore<User> userStore, User user, IConfigurationRoot configuration)
        {
            if (user == null)
            {
                return null;
            }

            var token = TokenGenerator.GenerateToken();

            user.EmailConfirmationTokenExpirationTime =
                DateTime.UtcNow.AddHours(int.Parse(configuration["Confirmation:EmailTokenLifeSpanInHours"]));
            user.EmailConfirmationToken = token;
            await userStore.UpdateAsync(user, CancellationToken.None);
            return token;
        }

        public static async Task<ConfirmationEmailResponseDto> ConfirmEmail(this UserManager<User> userManager, IUserStore<User> userStore,
            User user)
        {
            var confirmationEmailResponse = new ConfirmationEmailResponseDto();
            if (user == null)
            {
                return ConfirmationEmailUserNotFound(confirmationEmailResponse);
            }
            if (user.EmailConfirmed)
            {
                return ConfirmationEmailAlreadyConfirmed(confirmationEmailResponse);
            }
            if (user.EmailConfirmationTokenExpirationTime < DateTime.UtcNow)
            {
                return ConfirmationEmailTokenExpired(confirmationEmailResponse);
            }
            user.EmailConfirmed = true;
            var result = await userStore.UpdateAsync(user, CancellationToken.None);
            confirmationEmailResponse.Invalid = confirmationEmailResponse.Succedeed = result.Succeeded;
            return confirmationEmailResponse;
        }
        private static ConfirmationEmailResponseDto ConfirmationEmailTokenExpired(
            ConfirmationEmailResponseDto confirmationEmailResponse)
        {
            confirmationEmailResponse.Expired = true;
            return confirmationEmailResponse;
        }

        private static ConfirmationEmailResponseDto ConfirmationEmailAlreadyConfirmed(
            ConfirmationEmailResponseDto confirmationEmailResponse)
        {
            confirmationEmailResponse.AlreadyConfirmed = true;
            return confirmationEmailResponse;
        }

        private static ConfirmationEmailResponseDto ConfirmationEmailUserNotFound(
            ConfirmationEmailResponseDto confirmationEmailResponse)
        {
            confirmationEmailResponse.Invalid = true;
            return confirmationEmailResponse;
        }
    }
}
