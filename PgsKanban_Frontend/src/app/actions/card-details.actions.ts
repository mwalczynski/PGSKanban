import { Action } from '@ngrx/store';
import { CardDetails } from '../models/responses/board/cardDetails';
import { Comment } from '../models/responses/board/comment';

export const GET_CARD_DETAILS = 'GET_CARD_DETAILS';
export const CHANGE_LONG_DESCRIPTION = 'CHANGE_LONG_DESCRIPTION';
export const CHANGE_NAME = 'CHANGE_NAME';
export const ADD_COMMENT = 'ADD_COMMENT';

export class GetCardDetails implements Action {
    readonly type = GET_CARD_DETAILS;

    constructor(public payload: CardDetails) {
    }
}

export class ChangeLongDescription implements Action {
    readonly type = CHANGE_LONG_DESCRIPTION;

    constructor(public payload: string) {
    }
}

export class ChangeName implements Action {
    readonly type = CHANGE_NAME;

    constructor(public payload: string) {
    }
}


export class AddComment implements Action {
    readonly type = ADD_COMMENT;

    constructor(public payload: Comment) {
    }
}

export type CardDetailsActions = GetCardDetails | ChangeLongDescription | ChangeName | AddComment;
