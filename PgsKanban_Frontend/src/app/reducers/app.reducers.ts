import * as fromBoardList from './board.reducers';
import * as fromUserInfo from './user-info.reducers';
import * as fromMemberList from './member.reducers';
import * as fromDragStatus from './drag.reducers';
import * as fromStatistics from './statistics.reducers';
import * as fromCartDetails from './card-details.reducers';
import { ActionReducerMap } from '@ngrx/store';

export interface AppState {
    boardList: fromBoardList.State;
    userInfo: fromUserInfo.State;
    memberList: fromMemberList.State;
    dragStatus: fromDragStatus.State;
    cardDetails: fromCartDetails.State;
    statisticsShouldBeUpdated: fromStatistics.State;
}

export const reducers: ActionReducerMap<AppState> = {
    boardList: fromBoardList.boardReducer,
    userInfo: fromUserInfo.userInfoReducer,
    memberList: fromMemberList.memberReducer,
    dragStatus: fromDragStatus.dragStatusReducer,
    cardDetails: fromCartDetails.cardDetailsReducer,
    statisticsShouldBeUpdated: fromStatistics.statisticsReducer
};
