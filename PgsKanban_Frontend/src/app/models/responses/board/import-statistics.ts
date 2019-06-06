import { UserBoard } from './userboard';

export interface ImportStatistics {
    userBoard: UserBoard;
    listsCount: number;
    cardsCount: number;
    commentsCount: number;
}
