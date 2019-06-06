import {
    HttpErrorResponse,
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { ObservableInput } from 'rxjs/Observable';
import { Injectable, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ErrorModalComponent } from '../modals/error-modal/error-modal.component';
import { Url } from '../shared/constants';
import { ExternalLoginService } from '../services/external-login.service';

@Injectable()
export class ErrorResponseInterceptor implements HttpInterceptor {
    constructor(private dialog: MatDialog, private injector: Injector, private router: Router) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .catch((err: any, caught: Observable<any>): ObservableInput<any> => {
                if (err instanceof HttpErrorResponse) {
                    if (err.status === 0) {
                        this.dialog.open(ErrorModalComponent);
                    }
                    if (err.status === 401) {
                        const externalLoginService: ExternalLoginService = this.injector.get(ExternalLoginService);
                        externalLoginService.logout();
                    }
                    if (err.status === 500) {
                        this.dialog.open(ErrorModalComponent);
                    }
                    if (err.status === 403) {
                        this.router.navigateByUrl(Url.Forbidden, { skipLocationChange: true });
                    }
                    if (err.status === 404) {
                        this.router.navigateByUrl(Url.NotFound, { skipLocationChange: true });
                    }
                }
                return Observable.throw(err);
            });
    }
}