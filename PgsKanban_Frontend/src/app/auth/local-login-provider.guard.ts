import {Injectable} from '@angular/core';
import {CanActivate, Router} from '@angular/router';
import {LoginProvider} from '../models/enums/login-provider.enum';
import {ExternalLoginService} from '../services/external-login.service';

@Injectable()
export class LocalLoginProviderGuard implements CanActivate {
    constructor(private externalLoginService: ExternalLoginService, private router: Router) {
    }

    canActivate(): boolean {
        if (!this.externalLoginService.isProviderAvailable(LoginProvider.Local)) {
            this.externalLoginService.startOpenIdLogin();
            return false;
        } else {
            return true;
        }
    }
}
