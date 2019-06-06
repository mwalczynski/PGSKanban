export class SearchForUsersData {
    constructor(searchPhrase: string, boardId: number) {
        this.searchPhrase = searchPhrase;
        this.boardId = boardId;
    }
    searchPhrase: string;
    boardId: number;
}
