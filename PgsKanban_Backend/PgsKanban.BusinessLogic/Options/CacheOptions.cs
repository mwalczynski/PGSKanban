namespace PgsKanban.BusinessLogic.Options
{
    public class CacheOptions
    {
        public int ExpirationTimeInMinutes { get; set; }
        public int MaximumAmountOfAttemps { get; set; }
        public int RecoveryPasswordCaptchaExpirationInMinutes { get; set; }
        public int MaximumAmountOfAttempsRecovery { get; set; }
    }
}
