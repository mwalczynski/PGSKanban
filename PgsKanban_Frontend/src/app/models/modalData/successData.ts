export class SuccessData {
    constructor(public smallMessage: string) {
    }

    static default(): SuccessData {
        return new SuccessData('Your request has been processed');
    }

    static activated(): SuccessData {
        return new SuccessData('Your account has been activated');
    }

    static alreadyActivated(): SuccessData {
        return new SuccessData('Your account has been already activated');
    }

    static activationEmailSend(): SuccessData {
        return new SuccessData('Your account has been created. Please confirm it via the confirmation email we\'ve sent you.');
    }
}
