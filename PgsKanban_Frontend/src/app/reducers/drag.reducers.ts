import * as DragStatusActions from '../actions/drag-status.actions';

export interface State {
    draggingAllowed: boolean;
}

const InitialState: State = {
    draggingAllowed: true
};

export function dragStatusReducer(state = InitialState, action: DragStatusActions.DragStatusActions) {
    switch (action.type) {
        case DragStatusActions.ENABLE_DRAGGING:
            return {
                ...state,
                draggingAllowed: true
            };
        case DragStatusActions.DISABLE_DRAGGING:
            return {
                ...state,
                draggingAllowed: false
            };
        default:
            return state;
    }
}
