import {Injectable} from '@angular/core';
import {AddBoardData} from '../models/requests/board/addBoardData';
import {BASE_URL} from '../shared/constants';
import {Observable} from 'rxjs/Observable';
import {UserBoard} from '../models/responses/board/userboard';
import 'rxjs/add/operator/map';
import {HttpClient} from '@angular/common/http';
import {Members} from '../models/responses/board/members';
import {EditListPositionData} from '../models/requests/board/editListPositionData';


@Injectable()
export class BoardService {
    constructor(private http: HttpClient) {
    }

    addBoard(board: AddBoardData): Observable<UserBoard> {
        return this.http.post(`${BASE_URL}/boards`, board).map((response) => response as UserBoard);
    }

    editBoard(board) {
        return this.http.put(`${BASE_URL}/boards`, board);
    }

    getBoardByObfuscatedId(obfuscatedId: string) {
        return this.http.get(`${BASE_URL}/boards/getBoardByObfuscatedId/${obfuscatedId}`);
    }

    getBoardByCardId(cardObfuscatedId: string) {
        return this.http.get(`${BASE_URL}/boards/getBoardByCardId/${cardObfuscatedId}`);
    }

    hasBoards(): Observable<boolean> {
        return this.http.get(`${BASE_URL}/boards/exists`).map(res => res as boolean);
    }

    deleteBoard(id) {
        return this.http.delete(`${BASE_URL}/boards/delete/${id}`).map(res => res as number);
    }

    getBoards(): Observable<UserBoard[]> {
        return this.http.get(`${BASE_URL}/boards`).map(res => res as UserBoard[]);
    }

    changeFavorite(id) {
        return this.http.post(`${BASE_URL}/boards/favorite/${id}`, id).subscribe();
    }

    getMembers(boardId): Observable<Members> {
        return this.http.get(`${BASE_URL}/boards/member/${boardId}`).map(res => res as Members);
    }

    moveList(listId, boardId, newPosition) {
        const moveData: EditListPositionData = {
            id: listId,
            boardId: boardId,
            newPosition: newPosition
        };
        return this.http.put(`${BASE_URL}/lists/move`, moveData);
    }
}
