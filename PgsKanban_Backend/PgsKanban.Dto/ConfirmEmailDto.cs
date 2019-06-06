using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class ConfirmEmailDto
    {
        [Required]
        public string ConfirmationToken { get; set; }
    }
}
