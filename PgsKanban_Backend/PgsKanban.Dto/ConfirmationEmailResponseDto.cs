namespace PgsKanban.Dto
{
    public class ConfirmationEmailResponseDto
    {
        public bool Succedeed { get; set; }
        public bool Expired { get; set; }
        public bool Invalid { get; set; }
        public bool AlreadyConfirmed { get; set; }
    }
}
