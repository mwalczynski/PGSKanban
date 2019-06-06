using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set;
        }
        [Required]
        [MaxLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Given passwords have to be the same.")]
        public string ConfirmPassword { get; set; }
    }
}
