import {Injectable} from '@angular/core';
import {Resolve} from '@angular/router';
import {LoginProvider} from '../../models/enums/login-provider.enum';

import {Observable} from 'rxjs/Observable';

@Injectable()
export class GoogleResolverService implements Resolve<LoginProvider> {

    resolve(): LoginProvider {
        return LoginProvider.Google;
    }
}
