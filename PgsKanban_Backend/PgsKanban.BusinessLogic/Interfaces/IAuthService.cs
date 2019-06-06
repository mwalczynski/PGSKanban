using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PgsKanban.BusinessLogic.Enums;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> Login(LoginUserDto loginUserDto);
        Task<ReCaptchaResultRegisterDto> Register(RegisterUserDto registerUserDto);
        Task<TokenResponseDto> ExternalLogin(ExternalLoginDataDto externalData, ExternalLoginProvider loginProvider);
    }
}
