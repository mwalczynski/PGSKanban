using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface ICardService
    {
        CardDto CreateCard(AddCardDto cardDto, string userId);
        CardDto EditCard(EditCardNameDto cardDto, string userId);
        CardDetailsDto EditCardDescription(EditCardDescriptionDto cardDto, string userId);
        CardDto EditCardPosition(EditCardPositionDto cardDto, string userId);
        CardDetailsDto GetCardDetails(string obfuscatedId, string userId);
        CommentDto AddCommentToCard(CommentCardDto commentDto, string userId);
        CardDto DeleteCard(DeleteCardDto deleteCardDto, string userId);
    }
}
