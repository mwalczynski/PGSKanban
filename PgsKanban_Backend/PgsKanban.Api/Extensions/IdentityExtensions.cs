using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PgsKanban.Dto;

namespace PgsKanban.Api.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException();
            }
            var result = claimsPrincipal.FindFirst(x => x.Type == ClaimTypes.Email);
            return result.Value;
        }

        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException();
            }
            var result = claimsPrincipal.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);
            return result.Value;
        }
        public static ExternalLogOutDataDto GetExternalLogoutData(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException();
            }
            var token = claimsPrincipal.FindFirst(x => x.Type == JwtRegisteredClaimNames.CHash);
            var loginProvider = claimsPrincipal.FindFirst(x => x.Type == JwtRegisteredClaimNames.Typ);
            return new ExternalLogOutDataDto
            {
                LogoutToken = token?.Value,
                LoginProvider = loginProvider?.Value
            };
        }
    }
}
