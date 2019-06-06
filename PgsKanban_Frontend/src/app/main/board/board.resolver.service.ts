import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Board } from '../../models/responses/board/board';
import { BoardService } from '../../services/board.service';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class BoardResolverService implements Resolve<Board> {
    constructor(private boardService: BoardService) {
    }

    resolve(route: ActivatedRouteSnapshot): Observable<Board> {
        const obfuscatedId: string = route.params['obfuscatedId'];
        return this.boardService.getBoardByObfuscatedId(obfuscatedId).map(response => response as Board);
    }
}
