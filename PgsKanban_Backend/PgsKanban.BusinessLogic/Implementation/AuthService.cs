using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PgsKanban.BusinessLogic.Enums;
using PgsKanban.BusinessLogic.Services.ReCaptcha.Interfaces;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<User> _userManager;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfigurationRoot _configuration;
        private readonly IMapper _mapper;
        private readonly IReCaptchaValidation _reCaptchaValidation;

        public AuthService(UserManager<User> userManager, PasswordHasher<User> passwordHasher, IConfigurationRoot configuration, IMapper mapper,
                              IReCaptchaValidation reCaptchaValidation)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _mapper = mapper;
            _reCaptchaValidation = reCaptchaValidation;
        }

        public async Task<TokenResponseDto> Login(LoginUserDto loginUserDto)
        {
            var user = await GetUserWithBoards(loginUserDto.Email);
            var token = ConfirmUserPersonality(user, loginUserDto) ? GetJwtSecurityToken(user) : null;
            var tokenResponse = CreateTokenResponse(token, user);
            return tokenResponse;
        }

        private async Task<User> GetUserWithBoards(string email)
        {
            return await _userManager.Users.Include(x => x.Boards).ThenInclude(x => x.Board).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<ReCaptchaResultRegisterDto> Register(RegisterUserDto registerUserDto)
        {
            User user = _mapper.Map<RegisterUserDto, User>(registerUserDto);
            IdentityResult registrationResult = null;
            bool isCaptchaValidated = await _reCaptchaValidation.ValidateRecaptcha(registerUserDto.ReCaptchaToken);
            if (isCaptchaValidated)
            {
                user.RegisteredWith = ExternalLoginProvider.Local;
                registrationResult = await _userManager.CreateAsync(user, registerUserDto.Password);
            }
            var result = CreateResult(registrationResult, isCaptchaValidated);
            result.UserId = user.Id;
            return result;
        }

        public async Task<TokenResponseDto> ExternalLogin(ExternalLoginDataDto externalData, ExternalLoginProvider loginProvider)
        {
            var user = await GetUserWithBoards(externalData.ExternalUser.Email);
            if (user == null)
            {
                user = await RegisterUserFromExternalProvider(externalData, loginProvider);
            }
            if (user == null)
            {
                return null;
            }
            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }
            var token = GetJwtSecurityToken(user, loginProvider, externalData.AccessToken);
            var tokenResponse = CreateTokenResponse(token, user);
            return tokenResponse;
        }

        private async Task<User> RegisterUserFromExternalProvider(ExternalLoginDataDto externalData, ExternalLoginProvider loginProvider)
        {
            var user = _mapper.Map<ExternalUserDto, User>(externalData.ExternalUser);
            user.RegisteredWith = loginProvider;
            var result = await _userManager.CreateAsync(user);
            return result.Succeeded ? user : null;
        }

        private bool ConfirmUserPersonality(User user, LoginUserDto loginUserDto)
        {
            if (user?.PasswordHash == null)
            {
                return false;
            }
            var resultOfveryfyingPassword =
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);
            return resultOfveryfyingPassword == PasswordVerificationResult.Success;
        }

        private JwtSecurityToken GetJwtSecurityToken(User user, ExternalLoginProvider loginProvider = ExternalLoginProvider.Local, string externalToken = null)
        {
            return new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: GetTokenClaims(user, loginProvider, externalToken),
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["TokenConfiguration:TimeInMinutesOfJwtLife"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenConfiguration:Key"])), SecurityAlgorithms.HmacSha256)
            );
        }

        private IEnumerable<Claim> GetTokenClaims(User user, ExternalLoginProvider loginProvider, string externalToken)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id)
            };
            if (externalToken != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.CHash, externalToken));
                claims.Add(new Claim(JwtRegisteredClaimNames.Typ, loginProvider.ToString()));
            }
            return claims;
        }

        private TokenResponseDto CreateTokenResponse(JwtSecurityToken token, User user)
        {
            if (user == null || token == null)
            {
                return null;
            }
            var tokenResponse = new TokenResponseDto
            {
                IsAccountActive = user.EmailConfirmed,
                Token = !user.EmailConfirmed
                    ? null
                    : new TokenDto
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        BoardsExists = user.Boards!=null && user.Boards.Any(x => !x.IsDeleted
                                                         && !x.Board.IsDeleted
                                                         && x.LastTimeVisited > DateTime.MinValue)
                    }
            };
            return tokenResponse;
        }

        private ReCaptchaResultRegisterDto CreateResult(IdentityResult result, bool isCaptchaValidated)
        {
            var errors = result?.Errors.Select(x => x.Description).ToList();
            return new ReCaptchaResultRegisterDto
            {
                Errors = errors,
                ReCaptchaValidated = isCaptchaValidated,
                Succeeded = errors != null && !errors.Any() && isCaptchaValidated
            };
        }
    }
}
