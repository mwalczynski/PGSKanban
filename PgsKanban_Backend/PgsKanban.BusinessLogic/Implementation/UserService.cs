using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PgsKanban.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using PgsKanban.BusinessLogic.Options;
using Microsoft.Extensions.Options;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICardRepository _cardRepository;
        private readonly IListRepository _listRepository;
        private readonly IUserBoardRepository _userBoardRepository;
        private readonly IMapper _mapper;
        private readonly IObfuscator _obfuscator;
        private IHttpContextAccessor _accessor;
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        private readonly IPGeoOptions _configuration;



        public UserService(UserManager<User> userManager, IMapper mapper, ICardRepository cardRepository, IListRepository listRepository, IUserBoardRepository userBoardRepository,
            IObfuscator obfuscator, IHttpContextAccessor accessor, ILogger<UserService> logger, IOptions<IPGeoOptions> iPGeoOptions)
        {
            _userManager = userManager;
            _mapper = mapper;
            _cardRepository = cardRepository;
            _listRepository = listRepository;
            _userBoardRepository = userBoardRepository;
            _obfuscator = obfuscator;
            _accessor = accessor;
            _logger = logger;
            _configuration = iPGeoOptions.Value;
            _httpClient = new HttpClient();
        }

        public async Task<UserProfileDto> GetUserProfileByUserEmail(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            UserProfileDto userProfile = _mapper.Map<UserProfileDto>(user);

            return userProfile;
        }

        public async Task<SecureUserProfileDto> GetSecureUserProfileByUserEmail(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            var userDto = _mapper.Map<User, SecureUserProfileDto>(user, opts => opts.AfterMap(async (src, dest) =>
            {
                if (dest != null)
                {
                    dest.HasPassword = await _userManager.HasPasswordAsync(user);
                }
            }));
            return userDto;
        }

        public StatisticsDto GetStatistics(string userId)
        {
            var statistics = new StatisticsDto
            {
                Boards = _userBoardRepository.GetNumberOfBoards(userId),
                Lists = _listRepository.GetNumberOfLists(userId),
                Cards = _cardRepository.GetNumberOfCards(userId),
                Collaborators = _userBoardRepository.GetNumberOfCollaborators(userId)
            };
            var lastModified = _userBoardRepository.GetThreeLastVisitedBoards(userId);
            statistics.LatestUserBoards = _mapper.Map<ICollection<UserBoardDto>>(lastModified);
            foreach (var latestUserBoard in statistics.LatestUserBoards)
            {
                latestUserBoard.Board.ObfuscatedId = _obfuscator.Obfuscate(latestUserBoard.BoardId);
            }
            return statistics;
        }

        public ICollection<MemberDto> FindUsers(SearchForUsersDto searchForUsersDto, string userId)
        {
            var phrases = searchForUsersDto.SearchPhrase.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            var boardId = searchForUsersDto.BoardId;
            var queriedUsers = new List<User>();
            foreach (var phrase in phrases)
            {
                if (phrase == phrases[0])
                {
                    queriedUsers = _userManager.Users.Include(x => x.Boards)
                        .Where(x => !x.IsProfileAnonymous)
                        .Where(x => x.Id != userId && CheckConditionForSearchingPeople(x, phrase))
                        .ToList();
                }
                else
                {
                    queriedUsers = queriedUsers.Where(x => CheckConditionForSearchingPeople(x, phrase)).ToList();
                }
            }
            var result = queriedUsers.Select(user => PrepareFoundUser(user, boardId)).ToList();
            return result;
        }

        private MemberDto PrepareFoundUser(User user, int boardId)
        {
            return _mapper.Map<User, MemberDto>(user, opts =>
                opts.AfterMap((src, dest) =>
                    dest.Added = src.Boards.Any(x => x.BoardId == boardId && !x.IsDeleted)));
        }

        private bool CheckConditionForSearchingPeople(User user, string phrase)
        {
            return user.FirstName.StartsWith(phrase, StringComparison.OrdinalIgnoreCase) ||
                    user.LastName.StartsWith(phrase, StringComparison.OrdinalIgnoreCase) ||
                    user.NormalizedEmail.StartsWith(phrase, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IdentityResult> EditUserProfile(EditUserProfileDto editUserProfileDto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.FirstName = editUserProfileDto.FirstName;
            user.LastName = editUserProfileDto.LastName;
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<IdentityResult> EditUserAnonymousSettingsProfile(EditUserAnonymousSettingsDto editUserAnonymousSettingsDto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.IsProfileAnonymous = editUserAnonymousSettingsDto.IsProfileAnonymous;
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<string> GetUserLocalization()
        {
            var ipAddress = GetUserIp();

            try
            {
                var response = await _httpClient.GetAsync($"{_configuration.Provider}{ipAddress}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IPGeoDto>(json);
                    return $"{result.City}/{result.CountryName}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to retrive localization information for ip '{0}'", ipAddress);
            }
            return null;
        }

        public string GetUserIp()
        {
            return _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
