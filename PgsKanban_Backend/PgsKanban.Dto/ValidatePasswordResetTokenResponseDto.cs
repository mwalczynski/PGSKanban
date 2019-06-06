using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class ValidatePasswordResetTokenResponseDto
    {
        public bool Expired { get; set; }
        public bool Invalid { get; set; }
    }
}
