using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PgsKanban.Dto
{
    public class EditCardDescriptionDto
    {
        [Required]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
