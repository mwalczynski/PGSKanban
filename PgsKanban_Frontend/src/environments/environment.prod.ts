import {LoginProvider} from '../app/models/enums/login-provider.enum';

export const environment = {
    production: true,
    base_url: 'https://kanban-api.pgs-soft.com/api',
    base_hubs_url: 'https://kanban-api.pgs-soft.com/hubs',
    recaptcha_global_settings: { siteKey: '6LcOuyYTAAAAAHTjFuqhA52fmfJ_j5iFk5PsfXaU' },
    available_login_providers: [LoginProvider.OpenId]
};
