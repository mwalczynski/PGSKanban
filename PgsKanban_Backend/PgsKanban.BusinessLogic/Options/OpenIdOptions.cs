using System;
using PgsKanban.BusinessLogic.Enums;

namespace PgsKanban.BusinessLogic.Options
{
    public class OpenIdOptions : IProviderOptions
    {
        public ExternalLoginProvider LoginProvider => ExternalLoginProvider.OpenId;
        public string ResponseType { get; set; }
        public string RedirectUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string BaseUrl { get; set; }
        public string ValidIssuer { get; set; }
        public string JwksUrl { get; set; }
        public string ClientLoginEndpoint { get; set; }
        public string AuthorizeEndpoint => $"{BaseUrl}/authorize";
        public string PicturePrefix => "data:image/jpeg;base64,";
        public string LogOutEndpoint => $"{BaseUrl}/logout";
        public string TokenEndpoint => $"{BaseUrl}/token";
        public string UserInfoEndpoint => $"{BaseUrl}/userinfo";
        public string State => Guid.NewGuid().ToString("N");
        public string Nonce => Guid.NewGuid().ToString("N");
    }
}
