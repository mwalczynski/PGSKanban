export class ErrorData {
    constructor(public smallMessage: string, public buttonName: string, public handleButton?: () => any) {
    }

    static default(): ErrorData {
        return new ErrorData('Something went terribly wrong...', 'try again');
    }
    static expiredActivation(handleButton: () => any){
        return new ErrorData('Your activation link is expired', 'Resend activation email', handleButton);
    }
    static notActivated(handleButton: () => any){
        return new ErrorData('Your account has not been activated', 'Resend activation email', handleButton);
    }
    static invalidActivation() {
        return new ErrorData('Your activation link is not correct', 'Ok, cool');
    }
}
