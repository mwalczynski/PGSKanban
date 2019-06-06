import { Comment } from './comment';
export interface CardDetails {
    id?: number;
    obfuscatedId?: string;
    name: string;
    listName: string;
    description: string;
    comments: Comment[];
    isOwner?: boolean;
    boardId?: string;
    boardName?: string;
}
