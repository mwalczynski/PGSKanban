using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PgsKanban.Dto
{
    public class ExternalLoginInfoDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Provider { get; set; }
    }
}
