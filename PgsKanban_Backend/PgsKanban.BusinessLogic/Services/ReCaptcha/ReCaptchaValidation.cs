using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PgsKanban.BusinessLogic.Services.ReCaptcha.Interfaces;
using PgsKanban.Dto;


namespace PgsKanban.BusinessLogic.Services.ReCaptcha
{
    public class ReCaptchaValidation : IReCaptchaValidation
    {
        private readonly IConfigurationRoot _configuration;

        public ReCaptchaValidation(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> ValidateRecaptcha(string reCaptchaToken)
        {
            var secretKey = _configuration["RecaptchaValidation:SecretKey"];

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", secretKey),
                    new KeyValuePair<string, string>("response", reCaptchaToken)
                });

                var response = await client.PostAsync(_configuration["RecaptchaValidation:ReCaptchaUrl"], content);
                response.EnsureSuccessStatusCode();

                var stringResponse = await response.Content.ReadAsStringAsync();
                var reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(stringResponse);
                return reCaptchaResponse.Success;
            }
        }

        public ReCaptchaLoginResultDto CreateCaptchaLoginResponse(bool validated, bool shouldBeDisplayed, bool isAccountActive, bool invalidCredentials)
        {
            return new ReCaptchaLoginResultDto
            {
                InvalidCredentials = invalidCredentials,
                ReCaptchaValidated = validated,
                IsCaptchaDisplayed = shouldBeDisplayed,
                IsAccountActive = isAccountActive
            };
        }

        public ReCaptchaRecoveryResultDto CreateCaptchaResponseRecovery(bool validated, bool shouldBeDisplayed)
        {
            return new ReCaptchaRecoveryResultDto
            {
                ReCaptchaValidated = validated,
                IsCaptchaDisplayed = shouldBeDisplayed,
                Succeeded = false
            };
        }
    }
}
