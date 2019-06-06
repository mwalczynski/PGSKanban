import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import { BASE_URL } from '../../shared/constants';

import { RegisterUserData } from '../../models/requests/auth/registerUserData';
import { RegisterResult } from './../../models/responses/auth/registerResult';

@Injectable()
export class RegisterService {
    constructor(private http: HttpClient) {
    }

    register(data: RegisterUserData): Observable<any> {
        return this.http.post(`${BASE_URL}/account`, data)
            .map(res => res as RegisterResult)
            .catch(error => Observable.throw(error));
    }
}
