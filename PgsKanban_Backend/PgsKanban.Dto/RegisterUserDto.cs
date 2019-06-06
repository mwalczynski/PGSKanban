using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class RegisterUserDto
    {
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Given passwords have to be the same.")]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Terms and conditions must be accepted")]
        public bool AcceptedTerms { get; set; }      

        [Required]
        public string ReCaptchaToken { get; set; }

    }
}
