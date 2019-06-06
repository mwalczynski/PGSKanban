export class ConfirmEmailData {
    constructor(confirmationToken: string) {
        this.confirmationToken = confirmationToken;
    }
    confirmationToken: string;
}
