export class DeleteCardData {
    constructor(cardId: number, listId: number){
        this.cardId = cardId;
        this.listId = listId;
    }
    cardId: number;
    listId: number;
}
