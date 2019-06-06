import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/first';
import 'rxjs/add/observable/from';
import 'rxjs/add/operator/map';

import {LoginResult} from '../../models/responses/auth/loginResult';
import {HttpClient} from '@angular/common/http';
import {BASE_URL} from '../../shared/constants';
import { Token } from '../../models/responses/auth/token';

@Injectable()
export class LoginService {
    constructor(private http: HttpClient) {
    }

    login(userData): Observable<Token> {
        return this.http
            .post(`${BASE_URL}/account/token`, userData)
            .map(res => res as Token)
            .catch(res => {
                return Observable.throw(res.error as LoginResult);
            })
            .first();
    }

    isCaptchaDisplayed(): Observable<boolean> {
        return this.http.get(`${BASE_URL}/account/captcha`)
            .map(res => res as boolean)
            .first();
    }
}
