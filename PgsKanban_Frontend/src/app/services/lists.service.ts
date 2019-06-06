import { AddListData } from './../models/requests/board/addListData';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BASE_URL, LOCAL_STORAGE__LIST } from './../shared/constants';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { List } from '../models/responses/board/list';

@Injectable()
export class ListsService {
    headers: Headers;

    constructor(private http: HttpClient) {
        this.headers = new Headers({
            'Content-Type': 'application/json'
        });
    }

    saveLastListName(boardObfuscatedId, name) {
        if (!name) {
            localStorage.removeItem(LOCAL_STORAGE__LIST + boardObfuscatedId);
        } else {
            localStorage.setItem(LOCAL_STORAGE__LIST + boardObfuscatedId, name);
        }
    }

    getLastListName(boardObfuscatedId) {
        return localStorage.getItem(LOCAL_STORAGE__LIST + boardObfuscatedId);
    }

    getLists(): Observable<List[]> {
        return this.http.get(`${BASE_URL}/lists`).map(res => res as List[]);
    }

    addList(list: AddListData): Observable<List> {
        return this.http.post(`${BASE_URL}/lists`, list).map((res) => res as List);
    }

    deleteList(list) {
        return this.http
            .request('DELETE', `${BASE_URL}/lists`, { body: list })
            .map(res => res as Number);
    }

    editList(list) {
        return this.http.put(`${BASE_URL}/lists`, list);
    }

    moveCard(cardId, srcListId, targetListId, targetIndex) {
        const moveCardData = {
            'Id': cardId,
            'ListId': srcListId,
            'NewListId': targetListId,
            'NewPosition': targetIndex
        };
        return this.http.put(`${BASE_URL}/cards/move`, moveCardData);
    }
}
