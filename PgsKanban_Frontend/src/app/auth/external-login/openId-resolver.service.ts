import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { LoginProvider } from '../../models/enums/login-provider.enum';

import { Observable } from 'rxjs/Observable';

@Injectable()
export class OpenIdResolverService implements Resolve<LoginProvider> {

    resolve(): LoginProvider {
        return LoginProvider.OpenId;
    }
}
