
import { RecaptchaSettings } from 'ng-recaptcha';
import { LoginProvider } from '../models/enums/login-provider.enum';
import { environment } from '../../environments/environment';

export const BASE_URL =  environment.base_url;
export const BASE_HUBS_URL =  environment.base_hubs_url;
export const RECAPTCHA_GLOBAL_SETTINGS: RecaptchaSettings  =  environment.recaptcha_global_settings;
export const AVAILABLE_LOGIN_PROVIDERS: LoginProvider[] = environment.available_login_providers;
export const PASSWORD_REGEX = /^((?=.*[A-Z])(?=.*\W).{6,255})$/;
export const LOCAL_STORAGE__LIST = '_localLastListName_Board_';
export const ACTIVE_USERBOARD_CLASS = 'boardMiniature__active';
export const LOCAL_STORAGE__CARD = '_localLastCardName_Board_';
export const FACEBOOK_APP_ID = '1293843557429133';
export const FACEBOOK_VERSION = 'v2.8';
export const FACEBOOK_SCOPE = 'email,public_profile';
export const ACTIVATE_SOCKETS = false;

export const Url = {
    CardDetails: 'card',
    Board: 'board',
    AddBoard: 'board',
    Login: 'login',
    Register: 'register',
    ResetPassword: 'forgot',
    Forbidden: '403',
    NotFound: '404',
    Initial: '',
    Profile: 'profile'
};

export const Tooltip = {
    Delay: 500,
    BoardMiniatureNameLength: 24,
    BoardMiniatureOwnerLength: 20,
    MemberMiniatureNameLength: 14,
    MemberMiniatureEmailLength: 25,
    ListNameLength: 35,
    BoardNameLength: 75,
    CardDetailsCardNameLength: 60,
    CardDetailsListNameLength: 80
}

export const SPINNER_CONFIG = {
    primaryColour: '#FF5722',
    secondaryColour: '#FF5722',
    tertiaryColour: '#FF5722'
};

