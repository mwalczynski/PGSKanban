using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class SecureUserProfileDto: UserProfileDto
    {
        public bool HasPassword { get; set; }
    }
}
