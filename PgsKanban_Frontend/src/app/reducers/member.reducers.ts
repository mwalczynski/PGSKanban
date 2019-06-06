import * as MemberActions from '../actions/member.actions';
import {Member} from '../models/responses/board/member';

export interface State {
    members: Member[];
}

const InitialState: State = {
    members: []
};

export function memberReducer(state = InitialState, action: MemberActions.MemberActions) {
    let oldMemberList;
    let newMemberList;
    switch (action.type) {
        case MemberActions.GET_MEMBERS:
            return {
                ...state,
                members: [...action.payload]
            };
        case MemberActions.DELETE_MEMBER:
            oldMemberList = [...state.members];
            newMemberList = oldMemberList.filter(member => member.id !== action.payload);
            return {
                ...state,
                members: [...newMemberList]
            };
        case MemberActions.ADD_MEMBER:
            oldMemberList = [...state.members];
            return {
                ...state,
                members: [...oldMemberList, action.payload]
            };
        default:
            return state;
    }
}
