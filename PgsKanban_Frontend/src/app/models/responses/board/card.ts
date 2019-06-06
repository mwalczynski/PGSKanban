export interface Card {
    id?: number;
    obfuscatedId?: string;
    name: string;
    position?: number;
    listId: number;
    editingInProgress?: boolean;
    addingInProgress?: boolean;
}
