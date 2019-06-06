export interface LoginResult {
    invalidCredentials: boolean;
    isCaptchaDisplayed: boolean;
    errors: string[];
    reCaptchaValidated: boolean;
    isAccountActive: boolean;
}
