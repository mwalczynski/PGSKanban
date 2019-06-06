import { Injectable } from '@angular/core';
import { BASE_URL } from '../../shared/constants';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { RequestPasswordResetData } from '../../models/requests/profile/requestPasswordResetData';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class ResetPasswordService {

    constructor(private http: Http) {
    }

    isCaptchaDisplayed(): Observable<boolean> {
        return this.http.get(`${BASE_URL}/account/resetCaptcha`)
            .map(res => res.json());
    }

    resetPasswordRequest(data: RequestPasswordResetData): Observable<any> {
        return this.http.post(`${BASE_URL}/account/forgot`, data)
            .map(res => res.json())
            .catch((err) => {
                return Observable.throw(err.json());
            });
    }
}
