export class DeleteListData {
    constructor(listId: number, boardId: number){
        this.listId = listId;
        this.boardId = boardId;
    }
    listId: number;
    boardId: number;
}
