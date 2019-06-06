using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PgsKanban.BusinessLogic.Services;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.BusinessLogic.Extensions.UserManagerExtensions
{
    public static class ResetPasswordExtensions
    {
        public static async Task<string> SavePasswordResetToken(this UserManager<User> userManager, IUserStore<User> userStore, User user, int lifeTimeOfTokenInHours)
        {
            if (user == null)
            {
                return null;
            }
            var token = TokenGenerator.GenerateToken();
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpirationTime = DateTime.UtcNow.AddHours(lifeTimeOfTokenInHours);
            await userStore.UpdateAsync(user, CancellationToken.None);
            return token;
        }

        public static async Task<IdentityResult> ResetPassword(this UserManager<User> userManager, IUserStore<User> userStore,
            User user, string newPassword)
        {
            if (user.PasswordResetTokenExpirationTime < DateTime.UtcNow)
            {
                return null;
            }
            user.PasswordResetToken = null;
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, newPassword);
            var result = await userStore.UpdateAsync(user, CancellationToken.None);
            return result;
        }
    }
}
