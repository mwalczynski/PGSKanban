using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Interfaces
{
    public interface IBoardRepository
    {
        bool BoardExists(int boardId);
        bool IsOwner(int boardId, string userId);
        Board UpdateBoardName(Board board);
        Board DeleteBoard(int boardId);
    }
}
