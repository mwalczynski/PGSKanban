import { CommentCardData } from './../../models/requests/board/commentCardData';
import { EditCardDescriptionData } from './../../models/requests/board/editCardDescriptionData';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BASE_URL } from '../../shared/constants';

@Injectable()
export class CardDetailsService {

    constructor(private http: HttpClient) { }

    addComment(comment) {
        return this.http.post(`${BASE_URL}/cards/comment`, comment);
    }

    editCardDescription(card) {
        return this.http.put(`${BASE_URL}/cards/description`, card);
    }
}
