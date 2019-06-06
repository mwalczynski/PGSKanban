import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class AuthRequestInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.authService.token();
        if (token) {
            req = req.clone({ setHeaders: { Authorization: `Bearer ${token.toString()}` } });
        }
        return next.handle(req);
    }
}
