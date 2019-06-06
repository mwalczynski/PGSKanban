using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserProfileByUserEmail(string userEmail);
        Task<SecureUserProfileDto> GetSecureUserProfileByUserEmail(string userEmail);
        StatisticsDto GetStatistics(string userId);
        ICollection<MemberDto> FindUsers(SearchForUsersDto searchForUsersDto, string userId);
        Task<IdentityResult> EditUserProfile(EditUserProfileDto editUserProfileDto, string userId);
        Task<IdentityResult> EditUserAnonymousSettingsProfile(EditUserAnonymousSettingsDto editUserAnonymousSettingsDto, string userId);
        Task<string> GetUserLocalization();
        string  GetUserIp();
    }
}
