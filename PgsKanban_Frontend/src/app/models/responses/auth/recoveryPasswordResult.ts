export interface RecoveryPasswordResult {
    isCaptchaDisplayed: boolean;
    errors: string[];
    reCaptchaValidated: boolean;
    succeeded: boolean;
}
