using System.Linq;
using Microsoft.EntityFrameworkCore;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Implementation
{
    public class BoardRepository :BaseRepository, IBoardRepository
    {
        private readonly DbSet<Board> _boards;

        public BoardRepository(PgsKanbanContext context): base(context)
        {
            _boards = context.Boards;
        }

        public bool BoardExists(int boardId)
        {
            var result = _boards.Any(x => x.Id == boardId && x.IsDeleted == false);

            return result;
        }

        public Board UpdateBoardName(Board board)
        {
            _context.Attach(board);
            _context.Entry(board).Property(x => x.Name).IsModified = true;
            _context.SaveChanges();
            return board;
        }

        public bool IsOwner(int boardId, string userId)
        {
            var result = _boards.Any(x => x.Id == boardId
                                          && x.IsDeleted == false
                                          && x.OwnerId == userId);

            return result;
        }

        public Board DeleteBoard(int boardId)
        {
            var board = _boards.FirstOrDefault(x => x.Id == boardId);
            board.IsDeleted = true;
            _context.SaveChanges();
            return board;
        }
    }
}
