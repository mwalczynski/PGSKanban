export class AddCardData {
    constructor(name: string, listId: number) {
        this.name = name;
        this.listId = listId;
    }
    name: string;
    listId: number;
}
