import { Action } from '@ngrx/store';
import { UserBoard } from '../models/responses/board/userboard';
import { Board } from '../models/responses/board/board';
import { EditUserProfileData } from '../models/requests/profile/editUserProfileData';

export const ADD_BOARD = 'ADD_BOARD';
export const EDIT_BOARD = 'EDIT_BOARD';
export const DELETE_BOARD = 'DELETE_BOARD';
export const EDIT_BOARD_OWNER_NAME = 'EDIT_BOARD_OWNER_NAME';
export const GET_BOARDS = 'GET_BOARDS';
export const DELETE_MEMBER_FROM_BOARD = 'DELETE_MEMBER_FROM_BOARD';
export const ADD_MEMBER_TO_BOARD = 'ADD_MEMBER_TO_BOARD';
export const EDIT_ACTIVE_BOARD = 'EDIT_ACTIVE_BOARD';

export class AddBoard implements Action {
    readonly type = ADD_BOARD;

    constructor(public payload: UserBoard) {
    }
}

export class EditBoard implements Action {
    readonly type = EDIT_BOARD;

    constructor(public payload: Board) {
    }
}

export class DeleteBoard implements Action {
    readonly type = DELETE_BOARD;

    constructor(public payload: number) {
    }
}

export class GetBoards implements Action {
    readonly type = GET_BOARDS;

    constructor(public payload: UserBoard[]) {
    }
}

export class EditBoardOwnerName implements Action {
    readonly type = EDIT_BOARD_OWNER_NAME;

    constructor(public payload: EditUserProfileData) {
    }
}

export class EditActiveBoard implements Action {
    readonly type = EDIT_ACTIVE_BOARD;

    constructor(public payload: Board) {
    }
}

export class DeleteMemberFromBoard implements Action {
    readonly type = DELETE_MEMBER_FROM_BOARD;

    constructor(public payload: number) {
    }
}

export class AddMemberToBoard implements Action {
    readonly type = ADD_MEMBER_TO_BOARD;

    constructor(public payload: number) {
    }
}

export type BoardActions = AddBoard | EditBoard | DeleteBoard | GetBoards | EditBoardOwnerName |
    EditActiveBoard | DeleteMemberFromBoard | AddMemberToBoard;
