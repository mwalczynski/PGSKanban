using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class ChangePasswordUserDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
    }
}
