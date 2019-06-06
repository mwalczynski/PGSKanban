import { Observable } from 'rxjs/Observable';
import { ResetPasswordData } from './../../models/requests/profile/resetPasswordData';
import { Injectable } from '@angular/core';
import {Http} from '@angular/http';

import {BASE_URL} from '../../shared/constants';

@Injectable()
export class RecoverService {

  constructor(private http: Http) {
   }

   changePassword(data: ResetPasswordData): Observable<any> {
        return this.http.post(`${BASE_URL}/account/reset`, data)
            .map(res => res.json())
            .catch((err) => {
                return Observable.throw(err.json());
            });
    }

    validateToken(token) {
        return this.http.get(`${BASE_URL}/account/validate/${token}`)
        .map(res => res.json());
    }
}
