import * as CardDetailsActions from '../actions/card-details.actions';
import { CardDetails } from '../models/responses/board/cardDetails';

export interface State {
    cardDetails: CardDetails;
}

const InitialState: State = {
    cardDetails: undefined
};

export function cardDetailsReducer(state = InitialState, action: CardDetailsActions.CardDetailsActions) {
    let newCardDetails: CardDetails;
    switch (action.type) {
        case CardDetailsActions.GET_CARD_DETAILS:
            return {
                ...state,
                cardDetails: action.payload
            };
        case CardDetailsActions.CHANGE_LONG_DESCRIPTION:
            newCardDetails = {...state.cardDetails};
            newCardDetails.description = action.payload;
            return {
                ...state,
                cardDetails: newCardDetails
            };
        case CardDetailsActions.CHANGE_NAME:
            newCardDetails = {...state.cardDetails};
            newCardDetails.name = action.payload;
            return {
                ...state,
                cardDetails: newCardDetails
            };
        case CardDetailsActions.ADD_COMMENT:
            newCardDetails = {...state.cardDetails};
            newCardDetails.comments.unshift(action.payload)
            return {
                ...state,
                cardDetails: newCardDetails
            };
        default:
            return state;
    }
}
