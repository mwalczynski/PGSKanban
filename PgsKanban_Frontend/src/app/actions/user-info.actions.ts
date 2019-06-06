import {Action} from '@ngrx/store';
import {UserProfile} from '../models/responses/profile/userProfile';
import {EditUserProfileData} from '../models/requests/profile/editUserProfileData';

export const GET_USER_PROFILE = 'GET_USER_PROFILE';
export const EDIT_USER_PROFILE = 'EDIT_USER_PROFILE';

export class GetUserProfile implements Action {
    readonly type = GET_USER_PROFILE;

    constructor(public payload: UserProfile) {
    }
}

export class EditUserProfile implements Action {
    readonly type = EDIT_USER_PROFILE;

    constructor(public payload: EditUserProfileData) {
    }
}

export type UserInfoActions = GetUserProfile | EditUserProfile;
