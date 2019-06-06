import { Injectable } from '@angular/core';
import {
    AVAILABLE_LOGIN_PROVIDERS,
    BASE_URL,
    FACEBOOK_APP_ID,
    FACEBOOK_SCOPE,
    FACEBOOK_VERSION,
    Url
} from '../shared/constants';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { RedirectUrl } from '../models/responses/auth/redirectUrl';
import { LoginProvider } from '../models/enums/login-provider.enum';
import { Token } from '../models/responses/auth/token';
import { ExternalLoginData } from '../models/requests/auth/ExternalLoginData';
import { AuthService } from '../auth/auth.service';
import { LogoutResult } from '../models/responses/auth/logoutResult';
import { FacebookService, LoginResponse } from 'ngx-facebook';
import { Router } from '@angular/router';

@Injectable()
export class ExternalLoginService {
    constructor(private http: HttpClient,
        private authService: AuthService,
        private fb: FacebookService,
        private router: Router) {
        fb.init({
            version: FACEBOOK_VERSION,
            appId: FACEBOOK_APP_ID
        });
    }

    authorizeUser(data: ExternalLoginData): Observable<Token> {
        return this.http.post(`${BASE_URL}/external`, data).map(res => res as Token);
    }

    startOpenIdLogin() {
        const observable: Observable<RedirectUrl> = this.http.get(`${BASE_URL}/external`).map(res => res as RedirectUrl);
        observable.subscribe((redirectUrl) => {
            window.location.href = redirectUrl.url;
        });
    }

    startGoogleLogin() {
        const observable: Observable<RedirectUrl> = this.http.get(`${BASE_URL}/external/google`).map(res => res as RedirectUrl);
        observable.subscribe((redirectUrl) => {
            window.location.href = redirectUrl.url;
        });
    }
    startFacebook() {
        this.fb.login({
            scope: FACEBOOK_SCOPE
        }).then((loginResponse: LoginResponse) => {
            this.http.post(`${BASE_URL}/external/facebook`, loginResponse.authResponse)
                .map(res => res as Token).subscribe((tokenResponse) => {
                    this.authService.authorize(tokenResponse.token);
                    this.router.navigate([Url.Initial]);
                });
        });
    }

    isProviderAvailable(loginProvider: LoginProvider): boolean {
        return AVAILABLE_LOGIN_PROVIDERS.some(x => x === loginProvider);
    }

    isAnyExternalLoginProviderAvailable() {
        return AVAILABLE_LOGIN_PROVIDERS.some(x => x !== LoginProvider.Local);
    }

    logout() {
        this.http.post(`${BASE_URL}/external/logout`, {})
            .map(res => res as LogoutResult)
            .catch((err) => Observable.throw(err))
            .subscribe((logoutResult) => {
                if (logoutResult.externallyLoggedOut) {
                    this.authService.removeToken();
                    window.location.href = logoutResult.logOutUri;
                } else {
                    this.authService.logout();
                }
            });
    }
}
