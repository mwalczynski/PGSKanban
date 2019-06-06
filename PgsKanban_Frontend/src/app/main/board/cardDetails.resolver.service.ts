import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, Resolve} from '@angular/router';
import {CardsService} from '../card/card.service';
import {Observable} from 'rxjs/Observable';
import {CardDetails} from '../../models/responses/board/cardDetails';

@Injectable()
export class CardDetailsResolverService implements Resolve<CardDetails> {
    constructor(private cardService: CardsService) {
    }

    resolve(route: ActivatedRouteSnapshot): Observable<CardDetails> {
        const cardObfuscatedId: string = route.params['cardObfuscatedId'];
        return this.cardService.getCardDetails(cardObfuscatedId).map(response => response as CardDetails);
    }
}
