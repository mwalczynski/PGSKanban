import { Injectable } from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, Router} from '@angular/router';
import {Url} from '../../shared/constants';


@Injectable()
export class ExternalLoginGuard implements CanActivate {
    constructor(private router: Router) {}
  canActivate(route: ActivatedRouteSnapshot):  boolean {
     const canActivate =  route.queryParams['code'] && route.queryParams['state'];
     if (!canActivate) {
         this.router.navigate([Url.NotFound]);
         return false;
     }
     return true;
  }
}
