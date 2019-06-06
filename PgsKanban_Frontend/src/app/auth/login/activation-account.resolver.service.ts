import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, Resolve} from '@angular/router';
import {ConfirmationEmailResult} from '../../models/responses/auth/confirmationEmailResult';
import {Observable} from 'rxjs/Observable';
import {AuthService} from '../auth.service';
import {ActivationAccountService} from './activation-account.service';
import {ConfirmEmailData} from '../../models/requests/auth/confirmEmailData';

@Injectable()
export class ActivationAccountResolver implements Resolve<ConfirmationEmailResult>{
    resolve(route: ActivatedRouteSnapshot): Observable<ConfirmationEmailResult>{
        this.authService.removeToken();
        const token: string = route.params['token'];
        return this.activationService.confirmAccount(new ConfirmEmailData(token)).catch(err => {
            const confirmationResult = <ConfirmationEmailResult> err.json();
            confirmationResult.token = token;
            return Observable.of(confirmationResult);
        });
    }

    constructor(private authService: AuthService, private activationService: ActivationAccountService) { }

}
