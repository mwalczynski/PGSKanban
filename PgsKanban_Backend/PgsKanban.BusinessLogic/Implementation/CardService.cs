using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;
using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Implementation
{
    public class CardService : ICardService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IListRepository _listRepository;
        private readonly IMapper _mapper;
        private readonly IObfuscator _obfuscator;

        public CardService(ICommentRepository commentRepository, ICardRepository cardRepository, IListRepository listRepository, IMapper mapper, IObfuscator obfuscator)
        {
            _commentRepository = commentRepository;
            _cardRepository = cardRepository;
            _listRepository = listRepository;
            _mapper = mapper;
            _obfuscator = obfuscator;
        }

        public CardDto CreateCard(AddCardDto cardDto, string userId)
        {
            if (!_listRepository.IsOwner(cardDto.ListId, userId))
            {
                return null;
            }

            var card = _mapper.Map<Card>(cardDto);
            var position = _cardRepository.GetNumberOfCardsInList(cardDto.ListId) + 1;
            card.Position = position;
            var addedCard = _cardRepository.CreateCard(card);
            var result = _mapper.Map<CardDto>(addedCard);
            result.ObfuscatedId = _obfuscator.Obfuscate(result.Id);
            return result;
        }

        public CardDetailsDto GetCardDetails(string obfuscatedId, string userId)
        {
            var id = _obfuscator.Deobfuscate(obfuscatedId);

            if (!_cardRepository.IsMember(id, userId))
            {
                return null;
            }

            var card = _cardRepository.GetCardDetails(id);
            var result = _mapper.Map<CardDetailsDto>(card);
            for (int i = 0; i < card.Comments.Count; i++)
            {
                result.Comments.ElementAt(i).IsOwner = card.Comments.ElementAt(i).UserId == userId;
            }
            result.ObfuscatedId = _obfuscator.Obfuscate(result.Id);
            return result;
        }

        public CommentDto AddCommentToCard(CommentCardDto commentDto, string userId)
        {
            if (!_cardRepository.IsMember(commentDto.CardId, userId))
            {
                return null;
            }

            var comment = _mapper.Map<Comment>(commentDto);
            comment.UserId = userId;
            var addedComment = _commentRepository.CreateComment(comment);
            var result = _mapper.Map<CommentDto>(addedComment);
            result.IsOwner = true;
            return result;
        }

        public CardDto EditCard(EditCardNameDto cardDto, string userId)
        {
            if (!_cardRepository.IsOwner(cardDto.Id, userId))
            {
                return null;
            }

            var card = _mapper.Map<Card>(cardDto);
            var editedCard = _cardRepository.UpdateCardName(card);
            var result = _mapper.Map<CardDto>(editedCard);
            return result;
        }

        public CardDetailsDto EditCardDescription(EditCardDescriptionDto cardDto, string userId)
        {
            if (!_cardRepository.IsMember(cardDto.Id, userId))
            {
                return null;
            }

            var card = _mapper.Map<Card>(cardDto);
            var editedCard = _cardRepository.UpdateCardDescription(card);
            var result = _mapper.Map<CardDetailsDto>(editedCard);
            return result;
        }

        public CardDto EditCardPosition(EditCardPositionDto cardDto, string userId)
        {
            if (!_cardRepository.IsOwner(cardDto.Id, userId))
            {
                return null;
            }
            if (!_listRepository.IsMember(cardDto.ListId, userId) || !_listRepository.IsMember(cardDto.NewListId, userId))
            {
                return null;
            }

            var cardToEdit = _cardRepository.GetCard(cardDto.Id);
            if (cardToEdit.ListId != cardDto.ListId)
            {
                return null;
            }

            List<Card> cardsWithPositionToUpdate;

            if (cardDto.ListId != cardDto.NewListId)
            {
                cardsWithPositionToUpdate = GetCardsWithPositionToDecrease(cardToEdit.ListId, cardToEdit.Position);
                cardsWithPositionToUpdate.AddRange(GetCardsWithPositionToIncrease(cardDto.NewListId, cardDto.NewPosition - 1));
            }
            else
            {
                cardsWithPositionToUpdate = GetCardsWithPositionToUpdate(cardDto.ListId, cardToEdit.Position, cardDto.NewPosition);
            }

            cardToEdit.ListId = cardDto.NewListId;
            cardToEdit.Position = cardDto.NewPosition;
            cardsWithPositionToUpdate.Add(cardToEdit);
            _cardRepository.UpdateCardsPosition(cardsWithPositionToUpdate);

            var result = _mapper.Map<CardDto>(cardToEdit);
            return result;
        }

        public CardDto DeleteCard(DeleteCardDto deleteCardDto, string userId)
        {
            if (!_listRepository.IsOwner(deleteCardDto.ListId, userId))
            {
                return null;
            }

            var card = _cardRepository.GetCard(deleteCardDto.CardId);
            if(card == null)
            {
                return null;
            }

            var deletedCard = _cardRepository.DeleteCard(card);

            var cardsWithPositionToUpdate = GetCardsWithPositionToDecrease(deletedCard.ListId, deletedCard.Position);
            _cardRepository.UpdateCardsPosition(cardsWithPositionToUpdate);

            var result = _mapper.Map<CardDto>(deletedCard);
            return result;
        }

        private List<Card> GetCardsWithPositionToUpdate(int listId, int oldPosition, int newPosition)
        {
            List<Card> cardsWithPositionToUpdate;
            if (newPosition > oldPosition)
            {
                cardsWithPositionToUpdate = _cardRepository.GetCardsInPositionRange(listId, oldPosition, newPosition + 1);
                foreach (var card in cardsWithPositionToUpdate)
                {
                    card.Position--;
                }
            }
            else
            {
                cardsWithPositionToUpdate = _cardRepository.GetCardsInPositionRange(listId, newPosition - 1, oldPosition);
                foreach (var card in cardsWithPositionToUpdate)
                {
                    card.Position++;
                }
            }
            return cardsWithPositionToUpdate;
        }

        private List<Card> GetCardsWithPositionToDecrease(int listId, int position)
        {
            var cardsWithPositionToUpdate = _cardRepository.GetCardsWithGreaterPosition(listId, position);
            foreach (var card in cardsWithPositionToUpdate)
            {
                card.Position--;
            }
            return cardsWithPositionToUpdate;
        }

        private List<Card> GetCardsWithPositionToIncrease(int listId, int position)
        {
            var cardsWithPositionToUpdate = _cardRepository.GetCardsWithGreaterPosition(listId, position);
            foreach (var card in cardsWithPositionToUpdate)
            {
                card.Position++;
            }
            return cardsWithPositionToUpdate;
        }
    }
}
