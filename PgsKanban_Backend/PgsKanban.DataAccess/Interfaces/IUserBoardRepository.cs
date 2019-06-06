using System.Collections.Generic;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Interfaces
{
    public interface IUserBoardRepository
    {
        UserBoard CreateUserBoard(UserBoard userBoard);
        void UpdateBoardTimeVisited(UserBoard board);
        UserBoard GetUserBoard(int boardId, string userId);
        UserBoard GetUserBoardByCardId(int cardId, string userId);
        bool IsMember(int boardId, string userId);
        int GetNumberOfBoards(string userId);
        ICollection<UserBoard> GetThreeLastVisitedBoards(string userId);
        ICollection<UserBoard> GetUserBoards(string userId);
        UserBoard UpdatePriorityOfBoard(int boardId, string userId);
        ICollection<User> GetMembersOfBoard(int boardId);
        int GetNumberOfCollaborators(string userId);
        bool HasUserBoards(string userId);
        UserBoard ChangeUserBoardState(int boardId, string userId);
    }
}
