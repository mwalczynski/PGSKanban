using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Implementation
{
    public class CardRepository : BaseRepository, ICardRepository
    {
        private readonly DbSet<Card> _cards;

        public CardRepository(PgsKanbanContext context) : base(context)
        {
            _cards = context.Cards;
        }

        public Card CreateCard(Card card)
        {
            _cards.Add(card);
            _context.SaveChanges();
            return card;
        }

        public Card GetCard(int id)
        {
            var result = _cards.FirstOrDefault(x => x.Id == id
                                                    && !x.IsDeleted);
            return result;
        }

        public Card GetCardDetails(int id)
        {
            var result = _cards.Include(x => x.List)
                .Include(x => x.Comments).ThenInclude(x => x.User)
                .Include(x => x.Comments).ThenInclude(x => x.ExternalUser)
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            return result;
        }

        public Card UpdateCardName(Card card)
        {
            _context.Attach(card);
            _context.Entry(card).Property(x => x.Name).IsModified = true;
            _context.SaveChanges();
            return card;
        }

        public Card UpdateCardDescription(Card card)
        {
            _context.Attach(card);
            _context.Entry(card).Property(x => x.Description).IsModified = true;
            _context.SaveChanges();
            return card;
        }

        public bool IsOwner(int cardId, string userId)
        {
            var result = _cards.Any(x => x.Id == cardId
                                         && x.IsDeleted == false
                                         && x.List.Board.IsDeleted == false
                                         && x.List.Board.OwnerId == userId);

            return result;
        }

        public bool IsMember(int cardId, string userId)
        {
            var result = _cards.Any(x => x.Id == cardId
                                         && x.IsDeleted == false
                                         && x.List.IsDeleted == false
                                         && x.List.Board.IsDeleted == false
                                         && x.List.Board.Members.Any(y => y.UserId == userId));

            return result;
        }

        public bool CardExists(int cardId)
        {
            var result = _cards.Any(x => x.Id == cardId
                                         && x.IsDeleted == false
                                         && x.List.IsDeleted == false
                                         && x.List.Board.IsDeleted == false);

            return result;
        }

        public int GetNumberOfCards(string userId)
        {
            var result = _cards.Count(x => !x.List.Board.IsDeleted && !x.List.IsDeleted && !x.IsDeleted
                                           && x.List.Board.Members.Any(y => y.UserId == userId && !y.IsDeleted));
            return result;
        }

        public int GetNumberOfCardsInList(int listId)
        {
            var result = _cards.Count(x => x.ListId == listId && !x.IsDeleted);
            return result;
        }

        public List<Card> GetCardsWithGreaterPosition(int listId, int position)
        {
            var result = _cards.Where(x => !x.IsDeleted
                                           && x.ListId == listId
                                           && x.Position > position)
                .ToList();

            return result;
        }

        public List<Card> GetCardsInPositionRange(int listId, int startIndex, int endIndex)
        {
            var result = _cards.Where(x => !x.IsDeleted
                                           && x.ListId == listId
                                           && x.Position > startIndex
                                           && x.Position < endIndex)
                .ToList();

            return result;
        }

        public void UpdateCardsPosition(ICollection<Card> cards)
        {
            foreach (var card in cards)
            {
                _context.Attach(card);
                _context.Entry(card).Property(x => x.Position).IsModified = true;
                _context.Entry(card).Property(x => x.ListId).IsModified = true;
            }
            _context.SaveChanges();
        }

        public Card DeleteCard(Card card)
        {
            card.IsDeleted = true;
            _context.SaveChanges();
            return card;
        }
    }
}
