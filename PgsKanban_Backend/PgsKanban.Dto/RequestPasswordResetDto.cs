using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class RequestPasswordResetDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string RecaptchaResponse { get; set; }
    }
}
