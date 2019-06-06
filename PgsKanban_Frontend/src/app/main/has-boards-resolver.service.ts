import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import { BoardService } from '../services/board.service';

@Injectable()
export class HasBoardsResolver implements Resolve<boolean> {

    constructor(private hasBoardsService: BoardService) { }


    resolve(): Observable<boolean> {
        return this.hasBoardsService.hasBoards();
    }
}
