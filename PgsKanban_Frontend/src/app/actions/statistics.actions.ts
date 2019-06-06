import {Action} from '@ngrx/store';

export const STATISTICS_SHOULD_BE_UPDATED = 'STATISTICS_SHOULD_BE_UPDATED';
export const STATISTICS_UPDATED = 'STATISTICS_UPDATED';

export class StatisticsShouldBeUpdated implements Action {
    readonly type = STATISTICS_SHOULD_BE_UPDATED;
}

export class StatisticsUpdated implements Action {
    readonly type = STATISTICS_UPDATED;
}

export type StatisticsActions = StatisticsShouldBeUpdated | StatisticsUpdated;
