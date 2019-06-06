using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PgsKanban.BusinessLogic.Options;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IExternalLoginService
    {
        Task<ExternalLoginDataDto> GetExternalLoginData(ExternalLoginInfoDto externalLoginInfoDto, IPAddress ip);
        LogOutResultDto Logout(ExternalLogOutDataDto externalLogoutData);
        RedirectUrlDto GetOpenIdUrl(IPAddress ip);
        RedirectUrlDto GetGoogleUrl(IPAddress ip);
        Task<ExternalLoginDataDto> GetExternalFacebookLoginData(string loginInfoAccessToken);
    }
}
