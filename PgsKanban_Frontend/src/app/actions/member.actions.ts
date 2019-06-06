import {Action} from '@ngrx/store';
import {Member} from '../models/responses/board/member';

export const GET_MEMBERS = 'GET_MEMBERS';
export const ADD_MEMBER = 'ADD_MEMBER';
export const DELETE_MEMBER = 'DELETE_MEMBER';


export class GetMembers implements Action {
    readonly type = GET_MEMBERS;

    constructor(public payload: Member[]) {
    }
}

export class DeleteMember implements Action {
    readonly type = DELETE_MEMBER;

    constructor(public payload: string) {
    }
}

export class AddMember implements Action {
    readonly type = ADD_MEMBER;

    constructor(public payload: Member) {
    }
}

export type MemberActions = GetMembers | DeleteMember | AddMember;
