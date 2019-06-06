using System;
using System.Collections.Generic;
using System.Text;
using PgsKanban.BusinessLogic.Enums;

namespace PgsKanban.BusinessLogic.Options
{
    public interface IProviderOptions
    {
        ExternalLoginProvider LoginProvider { get; }
        string ResponseType { get; set; }
        string RedirectUrl { get; set; }
        string ClientId { get; set; }
        string Scope { get; set; }
        string BaseUrl { get; set; }
        string ClientSecret { get; set; }
        string TokenEndpoint { get; }
        string UserInfoEndpoint { get; }
        string State { get; }
        string Nonce { get; }
        string AuthorizeEndpoint { get; }
        string JwksUrl { get; set; }
        string ValidIssuer { get; set; }
        string ClientLoginEndpoint { get; set; }
    }
}
