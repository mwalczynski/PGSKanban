import { Injectable } from '@angular/core';


@Injectable()
export class PreviousRouteService {
    private readonly navigatedKey = 'navigated';

    saveNavigation(url: string): void {
        if (url !== '/404' ) {
            sessionStorage.setItem(this.navigatedKey, JSON.stringify(true));
        }
    }
    checkIfNavigatedBefore(): boolean {
        const navigated = sessionStorage.getItem(this.navigatedKey);
        return !!navigated;
    }
}
