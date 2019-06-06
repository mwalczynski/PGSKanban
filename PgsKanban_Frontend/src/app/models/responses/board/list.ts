import { Card } from './card';
export interface List {
    id?: number;
    name: string;
    position?: number;
    boardId: number;
    cards: Card[];
    isEditing?: boolean;
    addingInProgress?: boolean;
}
