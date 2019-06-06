import { ConfirmationData } from './confirmationData';

export class DeleteBoardConfirmationData implements ConfirmationData {
    message: string;

    constructor() {
        this.message = 'all lists and cards on this board';
    }
}
