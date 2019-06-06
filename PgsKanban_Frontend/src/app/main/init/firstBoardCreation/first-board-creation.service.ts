import { Injectable } from '@angular/core';
import { BASE_URL } from '../../../shared/constants';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { FirstBoardData } from '../../../models/requests/board/firstBoardData';
import { UserBoard } from '../../../models/responses/board/userboard';

@Injectable()
export class FirstBoardCreationService {

    constructor(private http: HttpClient) {
    }

    createInitBoard(firstBoard: FirstBoardData): Observable<UserBoard> {
        return this.http.post(`${BASE_URL}/boards/init`, firstBoard)
            .map(data => data as UserBoard);
    }
}
