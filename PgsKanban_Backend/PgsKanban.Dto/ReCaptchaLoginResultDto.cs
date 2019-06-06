using System.Collections.Generic;

namespace PgsKanban.Dto
{
    public class ReCaptchaLoginResultDto
    {
        public bool InvalidCredentials { get; set; }
        public bool IsCaptchaDisplayed { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool ReCaptchaValidated { get; set; }
        public bool IsAccountActive { get; set; }
    }
}
