export class EditListPositionData {
    constructor(id: number, boardId: number, newPosition: number) {
        this.id = id;
        this.boardId = boardId;
        this.newPosition = newPosition;
    }
    id: number;
    boardId: number;
    newPosition: number;
}
