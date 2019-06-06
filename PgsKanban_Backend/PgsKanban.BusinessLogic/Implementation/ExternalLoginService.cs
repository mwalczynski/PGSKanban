using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Facebook;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PgsKanban.BusinessLogic.Enums;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.BusinessLogic.OpenIdHelpers;
using PgsKanban.BusinessLogic.Options;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class ExternalLoginService : IExternalLoginService
    {
        private readonly OpenIdOptions _openIdOptions;
        private readonly GoogleOptions _googleOptions;
        private readonly FacebookOptions _facebookOptions;
        private readonly ICacheService _cacheService;

        public ExternalLoginService(IOptions<OpenIdOptions> openIdOptionsAccessor, ICacheService cacheService,
            IOptions<GoogleOptions> googleOptionsAccessor, IOptions<FacebookOptions> facebookOptionsAccessor)
        {
            _cacheService = cacheService;
            _openIdOptions = openIdOptionsAccessor.Value;
            _googleOptions = googleOptionsAccessor.Value;
            _facebookOptions = facebookOptionsAccessor.Value;

        }

        public RedirectUrlDto GetOpenIdUrl(IPAddress ip)
        {
            return GetAuthorizeUrl(ip, _openIdOptions);
        }

        public RedirectUrlDto GetGoogleUrl(IPAddress ip)
        {
            return GetAuthorizeUrl(ip, _googleOptions);
        }

        public async Task<ExternalLoginDataDto> GetExternalLoginData(ExternalLoginInfoDto externalLoginInfoDto, IPAddress ip)
        {
            var options = GetProviderOptions(externalLoginInfoDto);
            var client = new TokenClient(options.TokenEndpoint, options.ClientId, options.ClientSecret);
            var tokenResponse = await client.RequestAuthorizationCodeAsync(externalLoginInfoDto.Code, options.RedirectUrl);

            var validatedToken = await ValidateOpenIdToken(tokenResponse, externalLoginInfoDto.State, ip, options);

            if (!validatedToken)
            {
                return null;
            }

            var userInfo = await GetUserInfo(tokenResponse.AccessToken, options);
            return userInfo != null
                ?
                new ExternalLoginDataDto
                {
                    AccessToken = tokenResponse.AccessToken,
                    ExternalUser = userInfo
                }
                :
                null;
        }

       public LogOutResultDto Logout(ExternalLogOutDataDto externalLogoutData)
        {
            var result = new LogOutResultDto();
            if (externalLogoutData.LogoutToken == null)
            {
                return result;
            }
            var loginProvider = Enum.Parse<ExternalLoginProvider>(externalLogoutData.LoginProvider);
            string logoutUri;
            switch (loginProvider)
            {
                case ExternalLoginProvider.Facebook:
                {
                    logoutUri = CreateFacebookLogoutUri(externalLogoutData.LogoutToken);
                    break;
                }
                case ExternalLoginProvider.OpenId:
                {
                    logoutUri = CreateLogoutUri(externalLogoutData);
                    break;
                }
                case ExternalLoginProvider.Google:
                {
                    logoutUri = _googleOptions.LogOutEndpoint;
                    break;
                }
                default:
                {
                    return result;
                }
            }
            result.ExternallyLoggedOut = true;
            result.LogOutUri = logoutUri;
            return result;
        }

        public async Task<ExternalLoginDataDto> GetExternalFacebookLoginData(string loginInfoAccessToken)
        {
            var fb = new FacebookClient(loginInfoAccessToken);
            dynamic profile = await fb.GetTaskAsync(_facebookOptions.ProfileInfoUrl);

            var externalUser = CreateUserFromFacebook(profile);

            return new ExternalLoginDataDto
            {
                ExternalUser = externalUser,
                AccessToken = loginInfoAccessToken
            };
        }

        private string CreateLogoutUri(ExternalLogOutDataDto externalLogoutData)
        {
            return
                $"{_openIdOptions.LogOutEndpoint}?id_token_hint={externalLogoutData.LogoutToken}&post_logout_redirect_uri={_openIdOptions.ClientLoginEndpoint}&client_id={_openIdOptions.ClientId}";
        }

        private RedirectUrlDto GetAuthorizeUrl(IPAddress ip, IProviderOptions providerOptions)
        {
            var request = new AuthorizeRequest(providerOptions.AuthorizeEndpoint);
            var state = SaveState(ip);

            var url = request.CreateAuthorizeUrl(
                clientId: providerOptions.ClientId,
                responseType: providerOptions.ResponseType,
                scope: providerOptions.Scope,
                redirectUri: providerOptions.RedirectUrl,
                state: state);
            return new RedirectUrlDto
            {
                Url = url
            };
        }


        private string CreateFacebookLogoutUri(string logoutToken)
        {
            var fb = new FacebookClient(logoutToken);
            var logoutUri = fb.GetLoginUrl(new
            {
                client_id = _facebookOptions.AppId,
                redirect_uri = _facebookOptions.ClientLoginEndpoint,
            });
            return logoutUri.AbsoluteUri;
        }

        private IProviderOptions GetProviderOptions(ExternalLoginInfoDto externalLoginInfoDto)
        {
            var loginProvider = (ExternalLoginProvider)Enum.Parse(typeof(ExternalLoginProvider), externalLoginInfoDto.Provider);
            IProviderOptions options;

            if (loginProvider == ExternalLoginProvider.Google)
            {
                options = _googleOptions;
            }
            else
            {
                options = _openIdOptions;
            }

            return options;
        }

        private ExternalUserDto CreateUserFromFacebook(dynamic profile)
        {
            var email = profile[_facebookOptions.EmailKey];
            var firstname = profile[_facebookOptions.FirstNameKey];
            var lastname = profile[_facebookOptions.LastNameKey];

            var pictureSrc = GetPhotoFromFacebook(profile);

            var externalUser = new ExternalUserDto
            {
                Email = email,
                FirstName = firstname,
                LastName = lastname,
                PictureSrc = pictureSrc
            };
            return externalUser;
        }

        private string GetPhotoFromFacebook(dynamic profile)
        {
            JsonObject pictureData = profile[_facebookOptions.PictureKey];
            if (pictureData.TryGetValue(_facebookOptions.DataKey, out dynamic data))
            {
                if (data.TryGetValue(_facebookOptions.PictureUrlKey, out object picture))
                {
                    return picture as string;
                }
            }
            return null;
        }

        private async Task<ExternalUserDto> GetUserInfo(string accessToken, IProviderOptions options)
        {
            var userInfoClient = new UserInfoClient(options.UserInfoEndpoint);
            var userInfo = await userInfoClient.GetAsync(accessToken);
            return !userInfo.IsError ? CreateExternalUserFromClaims(userInfo.Claims.ToList(), options.LoginProvider) : null;
        }

        private ExternalUserDto CreateExternalUserFromClaims(ICollection<Claim> claims, ExternalLoginProvider externalLoginProvider)
        {
            ExternalUserDto externalUserDto;
            if (externalLoginProvider == ExternalLoginProvider.Google)
            {
                externalUserDto = GetUserFromGoogle(claims);
            }
            else
            {
                externalUserDto = GetUserFromOpenId(claims);
            }
            return externalUserDto;
        }

        private ExternalUserDto GetUserFromOpenId(ICollection<Claim> claims)
        {
            var email = claims.FirstOrDefault(x => x.Type == OpenIdClaim.Email.Description())?.Value;
            var firstname = claims.FirstOrDefault(x => x.Type == OpenIdClaim.FirstName.Description())?.Value;
            var lastname = claims.FirstOrDefault(x => x.Type == OpenIdClaim.LastName.Description())?.Value;
            var picture = GetOpenIdPicture(claims);

            var externalUserDto = CreateExternalUser(email, firstname, lastname, picture);
            return externalUserDto;
        }

        private string GetOpenIdPicture(ICollection<Claim> claims)
        {
            return $"{_openIdOptions.PicturePrefix}{claims.FirstOrDefault(x => x.Type == OpenIdClaim.Thumbnail.Description())?.Value}";
        }

        private static ExternalUserDto CreateExternalUser(string email, string firstname, string lastname, string pictureSrc)
        {
            var externalUser = new ExternalUserDto
            {
                Email = email,
                FirstName = firstname,
                LastName = lastname,
                PictureSrc = pictureSrc
            };
            return externalUser;
        }

        private static ExternalUserDto GetUserFromGoogle(ICollection<Claim> claims)
        {
            var email = claims.FirstOrDefault(x => x.Type == GoogleClaim.Email.Description())?.Value;
            var firstname = claims.FirstOrDefault(x => x.Type == GoogleClaim.FirstName.Description())?.Value;
            var lastname = claims.FirstOrDefault(x => x.Type == GoogleClaim.LastName.Description())?.Value;
            var picture = claims.FirstOrDefault(x => x.Type == GoogleClaim.Picture.Description())?.Value;

            var externalUserDto = CreateExternalUser(email, firstname, lastname, picture);
            return externalUserDto;
        }

        private async Task<bool> ValidateOpenIdToken(TokenResponse tokenResponse, string state, IPAddress ip, IProviderOptions options)
        {
            if (tokenResponse.IsError || !state.Equals(_cacheService.GetStateOpenId(ip), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var rsa = new RSACryptoServiceProvider();
            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadToken(tokenResponse.IdentityToken) as JwtSecurityToken;
            rsa.ImportParameters(await CreateRsaParameters(jsonToken?.Header.Kid, options));

            var parameters = CreateTokenValidatorParameters(rsa, options);

            try
            {
                handler.ValidateToken(
                    tokenResponse.IdentityToken, parameters, out _);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private async Task<RSAParameters> CreateRsaParameters(string kid, IProviderOptions options)
        {
            var key = await GetJwksInfo(kid, options);
            var keyExponent = GetKeyInBase64Format(key.KeyExponent);
            var keyModules = GetKeyInBase64Format(key.KeyModules);
            return new RSAParameters
            {
                Modulus = Convert.FromBase64String(keyModules),
                Exponent = Convert.FromBase64String(keyExponent)
            };
        }

        private static string GetKeyInBase64Format(string keyToFormat)
        {
            var key = keyToFormat.Replace('-', '+').Replace('_', '/');
            var formattedKeyPart = key.PadRight(key.Length + (4 - key.Length % 4) % 4, '=');

            return formattedKeyPart;
        }

        private TokenValidationParameters CreateTokenValidatorParameters(RSA rsa, IProviderOptions options)
        {
            var parameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateLifetime = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = options.ValidIssuer
            };
            return parameters;
        }
        private string SaveState(IPAddress ip)
        {
            var state = _openIdOptions.State;
            _cacheService.SaveProviderState(ip, state);
            return state;
        }

        private async Task<JwksKey> GetJwksInfo(string kid, IProviderOptions options)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(options.JwksUrl);
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<JwksData>(json);
                return data.Keys.FirstOrDefault(x => x.Kid == kid) ?? data.Keys.FirstOrDefault();
            }
        }
    }
}
