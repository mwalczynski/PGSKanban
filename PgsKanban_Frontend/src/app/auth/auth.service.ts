import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Url } from '../shared/constants';

export const TOKEN_NAME = 'jwt_token';

@Injectable()
export class AuthService {
    constructor(private router: Router) {
    }

    authorize(token: string) {
        localStorage.setItem(TOKEN_NAME, token);
    }

    removeToken() {
        localStorage.removeItem(TOKEN_NAME);
    }

    logout() {
        this.removeToken();
        this.router.navigate([Url.Login]);
    }

    isAuthorized() {
        return localStorage.getItem(TOKEN_NAME) != null;
    }

    token() {
        return localStorage.getItem(TOKEN_NAME);
    }
}
