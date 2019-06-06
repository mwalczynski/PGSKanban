using System.Collections.Generic;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Interfaces
{
    public interface ICardRepository
    {
        Card CreateCard(Card card);
        Card GetCard(int id);
        Card GetCardDetails(int id);
        Card UpdateCardName(Card card);
        Card UpdateCardDescription(Card card);
        bool IsOwner(int cardId, string userId);
        bool IsMember(int cardId, string userId);
        bool CardExists(int cardId);
        int GetNumberOfCards(string userId);
        int GetNumberOfCardsInList(int listId);
        List<Card> GetCardsWithGreaterPosition(int listId, int position);
        List<Card> GetCardsInPositionRange(int listId, int startIndex, int endIndex);
        void UpdateCardsPosition(ICollection<Card> cards);
        Card DeleteCard(Card card);
    }
}
