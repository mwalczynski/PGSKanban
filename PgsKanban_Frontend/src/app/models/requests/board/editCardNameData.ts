export class EditCardNameData {
    constructor(id: number, newName: string){
        this.id = id;
        this.name = newName;
    }
    id: number;
    name: string;
}
