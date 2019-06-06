import * as BoardActions from '../actions/board.actions';
import { UserBoard } from '../models/responses/board/userboard';
import { Board } from '../models/responses/board/board';

export interface State {
    userBoards: UserBoard[];
    activeBoard: Board;
}

const InitialState: State = {
    userBoards: [],
    activeBoard: undefined
};

export function boardReducer(state = InitialState, action: BoardActions.BoardActions) {
    let oldBoardList;
    let newState;
    switch (action.type) {
        case BoardActions.ADD_BOARD:
            return {
                ...state,
                userBoards: [...state.userBoards, action.payload]
            };
        case BoardActions.EDIT_BOARD:
            oldBoardList = [...state.userBoards];
            newState = oldBoardList.map((userBoard) => {
                if (userBoard.boardId === action.payload.id) {
                    userBoard.board.name = action.payload.name;
                }
                return userBoard;
            });
            return {
                ...state,
                userBoards: [...newState]
            };
        case BoardActions.DELETE_BOARD:
            oldBoardList = [...state.userBoards];
            newState = oldBoardList.filter(userBoard => userBoard.boardId !== action.payload);
            return {
                ...state,
                userBoards: [...newState]
            };
        case BoardActions.GET_BOARDS:
            return {
                ...state,
                userBoards: [...state.userBoards, ...action.payload]
            };
        case BoardActions.EDIT_BOARD_OWNER_NAME:
            oldBoardList = [...state.userBoards];
            newState = oldBoardList.map((userBoard) => {
                if (userBoard.isOwner) {
                    userBoard.board.owner.firstName = action.payload.firstName;
                    userBoard.board.owner.lastName = action.payload.lastName;
                }
                return userBoard;
            });
            return {
                ...state,
                userBoards: [...newState]
            };
        case BoardActions.EDIT_ACTIVE_BOARD:
            return {
                ...state,
                activeBoard: action.payload
            };
        case BoardActions.DELETE_MEMBER_FROM_BOARD:
            oldBoardList = [...state.userBoards];
            newState = oldBoardList.map((userBoard) => {
                if (userBoard.boardId === action.payload) {
                    userBoard.board.membersCount--;
                }
                return userBoard;
            });
            return {
                ...state,
                userBoards: [...newState]
            };
        case BoardActions.ADD_MEMBER_TO_BOARD:
            oldBoardList = [...state.userBoards];
            newState = oldBoardList.map((userBoard) => {
                if (userBoard.boardId === action.payload) {
                    userBoard.board.membersCount++;
                }
                return userBoard;
            });
            return {
                ...state,
                userBoards: [...newState]
            };
        default:
            return state;
    }
}
