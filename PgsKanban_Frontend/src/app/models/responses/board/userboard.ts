import { BoardMiniature } from './boardMiniature';
export interface UserBoard {
    userId: string;
    boardId: number;
    isOwner: boolean;
    isFavorite: boolean;
    board: BoardMiniature;
    lastTimeVisited: Date;
    lastTimeSetFavorite: Date;
}
