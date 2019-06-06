import {Injectable} from '@angular/core';
import {CanActivate, Router} from '@angular/router';

import {AuthService} from './auth.service';
import {Url} from '../shared/constants';

@Injectable()
export class NotAuthGuard implements CanActivate {
    constructor(private router: Router, private authService: AuthService) {
    }

    canActivate() {
        if (!this.authService.isAuthorized()) {
            return true;
        }

        this.router.navigate([Url.Initial]);
        return false;
    }
}
