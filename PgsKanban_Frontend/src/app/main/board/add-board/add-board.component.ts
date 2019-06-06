import { Component, OnInit } from '@angular/core';
import {AddBoardData} from '../../../models/requests/board/addBoardData';
import {Router} from '@angular/router';
import {BoardService} from '../../../services/board.service';
import {Url} from '../../../shared/constants';
import {Observable} from 'rxjs/Observable';
import {UserBoard} from '../../../models/responses/board/userboard';
import {Store} from '@ngrx/store';
import * as BoardActions from '../../../actions/board.actions';
import * as fromApp from '../../../reducers/app.reducers';

@Component({
  selector: 'app-add-board',
  templateUrl: './add-board.component.html',
  styleUrls: ['./add-board.component.scss']
})
export class AddBoardComponent implements OnInit {
    board: AddBoardData;
    pressed = false;
  constructor(private router: Router, private boardService: BoardService, private store: Store<fromApp.AppState>) { }

  ngOnInit() {
      this.board = new AddBoardData();
  }

    addBoard() {
        if (this.pressed) {
            return false;
        }
        this.pressed = true;
        this.boardService.addBoard(this.board).subscribe(userboard => {
                this.store.dispatch(new BoardActions.AddBoard(userboard));
                this.router.navigate([Url.Board, userboard.board.obfuscatedId, userboard.board.name]);
                this.pressed = false;
        });
    }

    redirectToHomePage() {
        this.router.navigate([Url.Login]);
    }

}
