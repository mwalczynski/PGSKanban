import { Component, OnInit } from '@angular/core';

import { LoginService } from './login.service';
import { LoginUserData } from '../../models/requests/auth/loginUserData';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationEmailResult } from '../../models/responses/auth/confirmationEmailResult';
import { ActivationAccountService } from './activation-account.service';
import { Token } from '../../models/responses/auth/token';
import { AuthService } from '../auth.service';
import { LoginResult } from '../../models/responses/auth/loginResult';
import { Url } from '../../shared/constants';
import { ExternalLoginService } from '../../services/external-login.service';
import { LoginProvider } from '../../models/enums/login-provider.enum';
import { Location } from '@angular/common';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    isCaptchaDisplayed: boolean;
    isError = false;
    errorMessage: string;
    isButtonDisabled: boolean;
    loginData: LoginUserData = new LoginUserData();

    constructor(private loginService: LoginService, private route: ActivatedRoute,
        private activationService: ActivationAccountService,
        private location: Location,
        private authService: AuthService, private router: Router,
        private externalLoginService: ExternalLoginService) {
        this.route.data.subscribe(val => {
            const confirmEmailData: ConfirmationEmailResult = val['confirmationEmailResult'];
            if (confirmEmailData === null || confirmEmailData) {
                this.activationService.handleActivation(confirmEmailData);
            } else {
                location.go(this.router.createUrlTree([Url.Login]).toString());
            }
        });
    }

    ngOnInit() {
        this.isError = false;
        this.shouldCaptchaBeDisplayed();
    }

    shouldCaptchaBeDisplayed() {
        this.loginService.isCaptchaDisplayed()
            .subscribe((isCaptchaDisplayed) => {
                this.isCaptchaDisplayed = isCaptchaDisplayed;
            });
    }

    onLogin(form: FormGroup) {
        this.isButtonDisabled = true;
        this.loginService.login(this.loginData)
            .subscribe((token: Token) => {
                this.handleLoginSuccess(token.token);
            }, (err: LoginResult) => {
                this.handleLoginRejection(form, err);
            });
    }

    handleLoginRejection(form: FormGroup, result: LoginResult) {
        this.isError = true;
        this.isButtonDisabled = false;
        this.handleCaptcha(result, form);
        this.handleLoginResult(result, form);
    }

    handleCaptcha(result: LoginResult, form: FormGroup) {
        this.isCaptchaDisplayed = result.isCaptchaDisplayed;
        if (form.controls.captcha) {
            form.controls.captcha.reset();
        }
    }

    cleanForm(form: FormGroup) {
        form.controls['password'].setErrors({ 'incorrect': true });
        this.loginData.password = '';
    }

    handleLoginResult(result: LoginResult, form: FormGroup) {
        if (!result.reCaptchaValidated) {
            this.cleanForm(form);
            this.errorMessage = 'Invalid captcha';
        } else if (result.invalidCredentials) {
            this.cleanForm(form);
            this.errorMessage = 'Email or password is incorrect';
        } else if (!result.isAccountActive) {
            this.activationService.showNotActivatedAccountPopup(this.loginData.email);
        }
    }

    handleLoginSuccess(token: string) {
        this.isError = false;
        this.isButtonDisabled = false;
        this.authService.authorize(token);
        this.router.navigate([Url.Initial]);
    }

    startOpenId() {
        this.externalLoginService.startOpenIdLogin();
    }

    startGoogleAuthentication() {
        this.externalLoginService.startGoogleLogin();
    }

    isOpenIdButtonDisplayed() {
        return this.externalLoginService.isProviderAvailable(LoginProvider.OpenId);
    }

    isGoogleButtonDisplayed() {
        return this.externalLoginService.isProviderAvailable(LoginProvider.Google);
    }

    isFacebookButtonDisplayed() {
        return this.externalLoginService.isProviderAvailable(LoginProvider.Facebook);
    }

    isAnyExternalLoginProviderAvailable() {
        return this.externalLoginService.isAnyExternalLoginProviderAvailable();
    }

    startFacebook() {
        return this.externalLoginService.startFacebook();
    }
}
