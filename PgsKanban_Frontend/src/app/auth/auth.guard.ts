import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { Url } from '../shared/constants';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router, private authService: AuthService) {
    }

    canActivate(): boolean {
        if (this.authService.isAuthorized()) {
            return true;
        }

        this.router.navigate([Url.Login]);
        return false;
    }
}
