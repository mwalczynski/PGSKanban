import { CardDetails } from './../../models/responses/board/cardDetails';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { AddCardData } from '../../models/requests/board/addCardData';
import { BASE_URL } from '../../shared/constants';
import { Card } from '../../models/responses/board/card';
import { LOCAL_STORAGE__CARD } from '../../shared/constants';

@Injectable()
export class CardsService {
    headers: Headers;

    constructor(private http: HttpClient) {
        this.headers = new Headers({
            'Content-Type': 'application/json'
        });
    }

    addCard(card: AddCardData): Observable<Card> {
        card.name = this.trimWhiteSpaces(card.name);
        return this.http.post(`${BASE_URL}/cards`, card).map((response) => response as Card);
    }

    getCards() {
        return this.http.get(`${BASE_URL}/cards`).map(res => res as Card[]);
    }

    saveLastCardName(boardId, name) {
        if (!name) {
            localStorage.removeItem(LOCAL_STORAGE__CARD + boardId);
        } else {
            localStorage.setItem(LOCAL_STORAGE__CARD + boardId, name);
        }
    }

    getLastCardName(boardId) {
        return localStorage.getItem(LOCAL_STORAGE__CARD + boardId);
    }

    trimWhiteSpaces(name) {
        return name.replace(/[\r\n\s]+/g, ' ');
    }

    editCard(card) {
        card.name = this.trimWhiteSpaces(card.name);
        return this.http.put(`${BASE_URL}/cards`, card);
    }

    deleteCard(card) {
        return this.http
            .request('DELETE', `${BASE_URL}/cards`, {body: card})
            .map(res => res as Number);
    }

    getCardDetails(cardId: string): Observable<CardDetails> {
        return this.http.get(`${BASE_URL}/cards/details/${cardId}`).map(res => res as CardDetails);
    }
}
