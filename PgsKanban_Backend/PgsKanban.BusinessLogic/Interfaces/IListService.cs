using PgsKanban.Dto;

namespace PgsKanban.BusinessLogic.Interfaces
{
    public interface IListService
    {
        ListDto CreateList(AddListDto listDto, string userId);
        ListDto GetList(int id, string userId);
        ListDto EditList(EditListDto listDto, string userId);
        ListDto EditListPosition(EditListPositionDto listDto, string userId);
        ListDto DeleteList(DeleteListDto deleteListDto, string userId);
    }
}
