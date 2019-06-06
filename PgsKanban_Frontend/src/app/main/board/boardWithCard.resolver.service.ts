import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Board } from '../../models/responses/board/board';
import { BoardService } from '../../services/board.service';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class BoardWithCardResolverService implements Resolve<Board> {
    constructor(private boardService: BoardService) {
    }

    resolve(route: ActivatedRouteSnapshot): Observable<Board> {
        const cardObfuscatedId: string = route.params['cardObfuscatedId'];
        return this.boardService.getBoardByCardId(cardObfuscatedId).map(response => response as Board);
    }
}
