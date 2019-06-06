namespace PgsKanban.BusinessLogic.Options
{
    public class MessageSenderOptions
    {
        public string AccountActivationTemplateName { get; set; }
        public string ResetPasswordTemplateName { get; set; }
        public string ChangePasswordTemplateName { get; set; }
        public int ServerPort { get; set; }
        public string ServerHost { get; set; }
    }
}
