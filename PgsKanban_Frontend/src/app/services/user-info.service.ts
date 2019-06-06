import {HttpParams, HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BASE_URL} from '../shared/constants';
import {UserProfile} from '../models/responses/profile/userProfile';
import {Statistics} from '../models/responses/profile/statistics';
import {Observable} from 'rxjs/Observable';
import * as UserInfoActions from '../actions/user-info.actions';
import 'rxjs/add/operator/map';
import {Store} from '@ngrx/store';
import * as fromApp from '../reducers/app.reducers';
import {Member} from '../models/responses/board/member';

@Injectable()
export class UserInfoService {
    constructor(private http: HttpClient, private store: Store<fromApp.AppState>) {
    }

    getUserProfile(): Observable<UserProfile> {
        return this.http.get(`${BASE_URL}/users/profile`).map(data => data as UserProfile);
    }

    searchUsers(searchPhrase, boardId) {
        let params = new HttpParams();
        params = params.append('searchPhrase', searchPhrase);
        params = params.append('boardId', boardId);

        return this.http.get(`${BASE_URL}/users/search`, {params: params}).map(data => data as Member[]);
    }

    editUserProfile(userData) {
        return this.http.post(`${BASE_URL}/users/edit`, userData);
    }

    changeUserAnonymity(newAnonymity) {
        const data = {
            isProfileAnonymous: newAnonymity,
        };
        return this.http.post(`${BASE_URL}/users/anonymous`, data);
    }

    getSecuredUserProfile(): Observable<UserProfile> {
        return this.http.get<UserProfile>(`${BASE_URL}/users/profile/secure`)
            .map(data => data as UserProfile)
            .do(data => this.store.dispatch(new UserInfoActions.GetUserProfile(data)));
    }

    getStatistics(): Observable<Statistics> {
        return this.http.get(`${BASE_URL}/users/statistics`).map(data => data as Statistics);
    }
}
