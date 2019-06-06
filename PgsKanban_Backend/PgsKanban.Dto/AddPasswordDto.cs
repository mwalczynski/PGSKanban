using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PgsKanban.Dto
{
    public class AddPasswordDto
    {
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Given passwords have to be the same.")]
        public string ConfirmNewPassword { get; set; }
    }
}
