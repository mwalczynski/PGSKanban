import {Injectable} from '@angular/core';
import {Resolve} from '@angular/router';

import {Observable} from 'rxjs/Observable';
import {UserProfile} from '../models/responses/profile/userProfile';
import {UserInfoService} from '../services/user-info.service';

@Injectable()
export class ProfileResolverService implements Resolve<UserProfile> {
    constructor(private userInfoService: UserInfoService) {
    }


    resolve(): Observable<UserProfile> {
        return this.userInfoService.getSecuredUserProfile();
    }
}
