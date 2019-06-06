// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

import {LoginProvider} from '../app/models/enums/login-provider.enum';

export const environment = {
    production: false,
    base_url: 'http://localhost:8888/api',
    base_hubs_url: 'http://localhost:8888/hubs',
    recaptcha_global_settings: { siteKey: '6LcOuyYTAAAAAHTjFuqhA52fmfJ_j5iFk5PsfXaU' },
    available_login_providers: [LoginProvider.OpenId, LoginProvider.Local, LoginProvider.Google, LoginProvider.Facebook]
};
