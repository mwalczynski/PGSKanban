namespace PgsKanban.BusinessLogic.Services.ReCaptcha
{
    public class ReCaptchaResponse
    {
        public bool Success { get; set; }
        public string Hostname { get; set; }
        public string[] Errorcodes { get; set; }
    }
}
