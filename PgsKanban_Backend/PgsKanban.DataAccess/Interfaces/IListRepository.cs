using System.Collections.Generic;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Interfaces
{
    public interface IListRepository
    {
        List CreateList(List list);
        List GetList(int id);
        List UpdateListName(List list);
        bool IsOwner(int listId, string userId);
        bool IsMember(int listId, string userId);
        int GetNumberOfLists(string userId);
        ICollection<List> GetListsInPositionRange(int boardId, int startIndex, int endIndex);
        void UpdateListsPosition(ICollection<List> lists);
        int GetNumberOfListsInBoard(int boardId);
        List DeleteList(List list);
        List<List> GetListsWithGreaterPosition(int boardId, int position);
    }
}
