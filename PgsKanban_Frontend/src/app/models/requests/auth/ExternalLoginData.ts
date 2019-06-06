import { LoginProvider } from '../../enums/login-provider.enum';

export class ExternalLoginData {
    code: string;
    state: string;
    provider: string;
    constructor(code: string, state: string) {
        this.code = code;
        this.state = state;
    }
}
