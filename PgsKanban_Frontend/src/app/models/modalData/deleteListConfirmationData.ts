import { ConfirmationData } from './confirmationData';

export class DeleteListConfirmationData implements ConfirmationData {
    message: string;

    constructor() {
        this.message = 'all cards in this list';
    }
}
