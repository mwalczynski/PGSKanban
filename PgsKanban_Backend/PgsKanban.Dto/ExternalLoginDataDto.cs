using System;
using System.Collections.Generic;
using System.Text;
using IdentityModel.Client;

namespace PgsKanban.Dto
{
    public class ExternalLoginDataDto
    {
        public string AccessToken { get; set; }
        public ExternalUserDto ExternalUser { get; set; }
    }
}
