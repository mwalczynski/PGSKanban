import { ConfirmationData } from './confirmationData';

export class DeleteCardConfirmationData implements ConfirmationData {
    message: string;

    constructor() {
        this.message = 'content of this card';
    }
}
