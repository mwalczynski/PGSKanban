import {Injectable} from '@angular/core';
import {Resolve} from '@angular/router';

import {Observable} from 'rxjs/Observable';
import {Store} from '@ngrx/store';
import {BoardService} from '../services/board.service';
import * as fromApp from '../reducers/app.reducers';
import * as BoardActions from '../actions/board.actions';

@Injectable()
export class MainResolverService implements Resolve<any> {
    constructor(private store: Store<fromApp.AppState>, private boardsService: BoardService) {
    }


    resolve(): void {
        this.getUserBoards();
    }

    getUserBoards() {
        this.boardsService.getBoards().subscribe(data => {
            this.store.dispatch(new BoardActions.GetBoards(data));
        });
    }
}
