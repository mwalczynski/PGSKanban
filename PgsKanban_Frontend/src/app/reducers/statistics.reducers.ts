import * as StatisticsActions from '../actions/statistics.actions';

export interface State {
    shouldStatisticsBeUpdated: boolean;
}

const InitialState: State = {
    shouldStatisticsBeUpdated: true
};

export function statisticsReducer(state = InitialState, action: StatisticsActions.StatisticsActions) {
    switch (action.type) {
        case StatisticsActions.STATISTICS_SHOULD_BE_UPDATED:
            return {
                ...state,
                shouldStatisticsBeUpdated: true
            };
        case StatisticsActions.STATISTICS_UPDATED:
            return {
                ...state,
                shouldStatisticsBeUpdated: false
             };
        default:
            return state;
    }
}
