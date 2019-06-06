using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class ExternalUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PictureSrc { get; set; }
    }
}
