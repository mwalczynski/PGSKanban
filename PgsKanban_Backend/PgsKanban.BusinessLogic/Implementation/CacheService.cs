using System;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.BusinessLogic.Options;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class CacheService : ICacheService
    {
        private readonly MemoryCacheEntryOptions _optionsLogin;
        private readonly MemoryCacheEntryOptions _optionsRecovery;
        private readonly CacheOptions _cacheOptions;
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache, IOptions<CacheOptions> cacheOptions)
        {
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _optionsLogin = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheOptions.ExpirationTimeInMinutes));
            _optionsRecovery = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheOptions.RecoveryPasswordCaptchaExpirationInMinutes));
        }

        public bool IsNumberOfAttempsExceeded(string ip)
        {
            if (!_cache.TryGetValue(ip, out int remainingAttempts))
            {
                return false;
            }
            return remainingAttempts <= 0;
        }

        public bool IsNumberOfAttempsExceededRecovery(IPAddress ip)
        {
            if (!_cache.TryGetValue(CreateResetCaptchaKey(ip), out int remainingAttemptsRecovery))
            {
                return false;
            }
            return remainingAttemptsRecovery <= 0;
        }

        public void UpdateFailedAttempsCount(string ip)
        {
            if (_cache.TryGetValue(ip, out int oldAttemps))
            {
                _cache.Set(ip, --oldAttemps);
                return;
            }
            _cache.Set(ip, _cacheOptions.MaximumAmountOfAttemps-1, _optionsLogin);
        }

        public void UpdateAttempsCountRecovery(IPAddress recoveryIp)
        {
            var key = CreateResetCaptchaKey(recoveryIp);
            if (_cache.TryGetValue(key, out int oldAttempsRecovery))
            {
                _cache.Set(key, --oldAttempsRecovery);
                return;
            };
            _cache.Set(key, _cacheOptions.MaximumAmountOfAttempsRecovery - 1, _optionsRecovery);
        }

        public bool HandleLoginAttemps(IPAddress remoteIpAddress)
        {
            UpdateFailedAttempsCount(remoteIpAddress.ToString());

            var result = IsNumberOfAttempsExceeded(remoteIpAddress.ToString());
            return result;
        }

        public bool HandleAttempsRecovery(IPAddress remoteIpAddressRecovery)
        {
            UpdateAttempsCountRecovery(remoteIpAddressRecovery);

            var result = IsNumberOfAttempsExceededRecovery(remoteIpAddressRecovery);
            return result;
        }
        public void SaveProviderState(IPAddress ip, string state)
        {
            _cache.Set(CreateStateOpenIdKey(ip), state);
        }

        public string GetStateOpenId(IPAddress ip)
        {
            var key = CreateStateOpenIdKey(ip);
            _cache.TryGetValue(key, out string state);
            _cache.Remove(key);
            return state;
        }

        private string CreateStateOpenIdKey(IPAddress ip)
        {
            return $"{ip}state";
        }
        private string CreateResetCaptchaKey(IPAddress ip)
        {
            return $"{ip}Recovery";
        }
    }
}
