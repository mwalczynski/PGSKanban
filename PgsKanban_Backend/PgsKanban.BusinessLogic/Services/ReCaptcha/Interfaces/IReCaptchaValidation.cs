using System.Threading.Tasks;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Services.ReCaptcha.Interfaces
{
    public interface IReCaptchaValidation
    {
        Task<bool> ValidateRecaptcha(string reCaptchaToken);       
        ReCaptchaRecoveryResultDto CreateCaptchaResponseRecovery(bool validated, bool shouldBeDisplayed);
        ReCaptchaLoginResultDto CreateCaptchaLoginResponse(bool validated, bool shouldBeDisplayed, bool isAccountActive, bool invalidCredentials);
    }
}
