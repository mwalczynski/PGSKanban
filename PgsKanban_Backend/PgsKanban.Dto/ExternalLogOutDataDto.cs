using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class ExternalLogOutDataDto
    {
        public string LogoutToken { get; set; }
        public string LoginProvider { get; set; }
    }
}
