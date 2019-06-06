using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class LogOutResultDto
    {
        public bool ExternallyLoggedOut { get; set; }
        public string LogOutUri { get; set; }
    }
}
