import {Action} from '@ngrx/store';

export const ENABLE_DRAGGING = 'ENABLE_DRAGGING';
export const DISABLE_DRAGGING = 'DISABLE_DRAGGING';


export class EnableDragging implements Action {
    readonly type = ENABLE_DRAGGING;
}

export class DisableDragging implements Action {
    readonly type = DISABLE_DRAGGING;
}

export type DragStatusActions = EnableDragging | DisableDragging;
