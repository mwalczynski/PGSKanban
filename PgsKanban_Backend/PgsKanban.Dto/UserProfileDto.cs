namespace PgsKanban.Dto
{
    public class UserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsProfileAnonymous { get; set; }
		public string HashMail { get; set; }
        public string Email { get; set; }
        public string PictureSrc { get; set; }
    }
}
