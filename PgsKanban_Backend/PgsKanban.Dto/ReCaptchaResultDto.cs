using System.Collections.Generic;

namespace PgsKanban.Dto
{
    public class ReCaptchaResultDto
    {
        public IEnumerable<string> Errors { get; set; }
        public bool ReCaptchaValidated { get; set; }
        public bool Succeeded { get; set; }
    }
}
