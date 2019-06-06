import {LoginProvider} from '../app/models/enums/login-provider.enum';

export const environment = {
    production: false,
    base_url: 'http://10.10.70.244:4001/api',
    base_hubs_url: 'http://10.10.70.244:4001/hubs',
    recaptcha_global_settings: { siteKey: '6LcOuyYTAAAAAHTjFuqhA52fmfJ_j5iFk5PsfXaU' },
    available_login_providers: [LoginProvider.OpenId, LoginProvider.Local, LoginProvider.Google, LoginProvider.Facebook]
};
