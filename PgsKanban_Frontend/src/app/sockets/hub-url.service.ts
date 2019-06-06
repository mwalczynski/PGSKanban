import { AuthService } from '../auth/auth.service';
import { BASE_HUBS_URL } from '../shared/constants';
import { Injectable } from '@angular/core';

@Injectable()
export class HubUrlService {
    constructor(private authService: AuthService) {
    }

    createUrl(hub: string) {
        const token = this.authService.token();
        return `${BASE_HUBS_URL}/${hub}?token=${token}`;
    }
}
