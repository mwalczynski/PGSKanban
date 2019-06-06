using System.Net;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface ICacheService
    {
        bool IsNumberOfAttempsExceeded(string ip);
        void UpdateFailedAttempsCount(string ip);
        bool HandleLoginAttemps(IPAddress remoteIpAddress);
        void UpdateAttempsCountRecovery(IPAddress recoveryIp);
        bool HandleAttempsRecovery(IPAddress remoteIpAddressRecovery);
        bool IsNumberOfAttempsExceededRecovery(IPAddress ip);
        void SaveProviderState(IPAddress ip, string state);
        string GetStateOpenId(IPAddress ip);
    }
}
