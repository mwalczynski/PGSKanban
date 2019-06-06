import * as UserInfoActions from '../actions/user-info.actions';
import {UserProfile} from '../models/responses/profile/userProfile';

export interface State {
    userProfile: UserProfile;
}

const InitialState: State = {
    userProfile: {
        firstName: '',
        lastName: '',
        isProfileAnonymous: false,
        hashMail: '',
        email: '',
        hasPassword: false
    }
};

export function userInfoReducer(state = InitialState, action: UserInfoActions.UserInfoActions) {
    switch (action.type) {
        case UserInfoActions.GET_USER_PROFILE:
            return {
                ...state,
                userProfile: action.payload
            };
        case UserInfoActions.EDIT_USER_PROFILE:
            const newUserProfile = {...state.userProfile};

            newUserProfile.firstName = action.payload.firstName;
            newUserProfile.lastName = action.payload.lastName;
            return {
                ...state,
                userProfile: newUserProfile
            };
        default:
            return state;
    }
}
