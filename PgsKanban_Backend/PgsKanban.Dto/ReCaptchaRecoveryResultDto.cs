using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class ReCaptchaRecoveryResultDto: ReCaptchaResultDto
    {
        public bool IsCaptchaDisplayed { get; set; }
    }
}
