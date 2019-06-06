using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IChangePasswordService
    {
        Task<string> SendResetLink(string userEmail);
        Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<IdentityResult> ChangePassword(ChangePasswordUserDto changePasswordUserDto, string userId);
        Task<ValidatePasswordResetTokenResponseDto> ValidateToken(string token);
        Task<IdentityResult> AddPassword(AddPasswordDto addPasswordDto, string userId);
        Task SendPasswordChangeInformation(string userEmail, string userBrowser, string userBrowserVersion);
    }
}
