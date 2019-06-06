export interface RegisterResult {
    userId: string;
    errors: string[];
    reCaptchaValidated: boolean;
    succeeded: boolean;
}
