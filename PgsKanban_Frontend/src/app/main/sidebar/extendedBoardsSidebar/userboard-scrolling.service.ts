import {ElementRef, Injectable} from '@angular/core';
import { ACTIVE_USERBOARD_CLASS } from '../../../shared/constants';

const boardMiniatureHeight = 159;

@Injectable()
export class UserboardScrollingService {

    scrollToActiveBoardMiniature(elementRef) {
        const activeBoard = this.getActiveUserBoardElement();
        if (activeBoard) {
            elementRef.nativeElement.scrollBy({
                top: this.computeYOffset(activeBoard),
                left: 0,
                behavior: 'smooth'
            });
        }
    }

    scrollTop(elementRef: ElementRef) {
        elementRef.nativeElement.scrollBy(0, -boardMiniatureHeight);
        setTimeout(elementRef.nativeElement.scrollBy({
            top: - elementRef.nativeElement.scrollHeight,
            left: 0,
            behavior: 'smooth'
        }));
    }

    private getActiveUserBoardElement(): HTMLInputElement {
        const activeBoard = document.getElementsByClassName(ACTIVE_USERBOARD_CLASS)[0];
        return activeBoard as HTMLInputElement;
    }

    private computeYOffset(activeBoard: HTMLInputElement) {
        return activeBoard.getBoundingClientRect().top - activeBoard.getBoundingClientRect().height;
    }
}
