using System;
using System.Collections.Generic;
using System.Text;
using PgsKanban.BusinessLogic.Enums;

namespace PgsKanban.BusinessLogic.Options
{
    public class GoogleOptions : IProviderOptions
    {
        public string AuthorizeEndpoint => $"{BaseUrl}";
        public ExternalLoginProvider LoginProvider => ExternalLoginProvider.Google;
        public string ResponseType { get; set; }
        public string RedirectUrl { get; set; }
        public string ClientId { get; set; }
        public string Scope { get; set; }
        public string BaseUrl { get; set; }
        public string ClientSecret { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserInfoEndpoint { get; set; }
        public string State => Guid.NewGuid().ToString("N");
        public string Nonce => Guid.NewGuid().ToString("N");
        public string JwksUrl { get; set; }
        public string ValidIssuer { get; set; }
        public string ClientLoginEndpoint { get; set; }
        public string LogOutEndpoint => $"https://accounts.google.com/Logout?&continue={ClientLoginEndpoint}";

    }
}
