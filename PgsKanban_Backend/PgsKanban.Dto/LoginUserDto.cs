using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class LoginUserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string RecaptchaResponse { get; set; }
    }
}