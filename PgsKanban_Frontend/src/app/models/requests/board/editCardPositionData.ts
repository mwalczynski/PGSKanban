export class EditCardPositionData {
    constructor(id: number, listId: number, newListId: number, newPosition: number) {
        this.id = id;
        this.listId = listId;
        this.newListId = newListId;
        this.newPosition = newPosition;
    }
    id: number;
    listId: number;
    newListId: number;
    newPosition: number;
}
