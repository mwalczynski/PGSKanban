export interface ConfirmationEmailResult {
    succedeed: boolean;
    expired: boolean;
    invalid: boolean;
    alreadyConfirmed: boolean;
    token?: string;
}
