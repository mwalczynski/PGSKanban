using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Implementation
{
    public class UserBoardRepository : BaseRepository, IUserBoardRepository
    {
        private readonly DbSet<UserBoard> _userBoards;

        public UserBoardRepository(PgsKanbanContext context) : base(context)
        {
            _userBoards = context.UserBoards;
        }

        public UserBoard GetUserBoard(int boardId, string userId)
        {
            var result = _userBoards
                .Include(x => x.Board)
                .ThenInclude(x => x.Lists)
                .ThenInclude(x => x.Cards)
                .Where(x => !x.Board.IsDeleted)
                .FirstOrDefault(x => x.BoardId == boardId && x.UserId == userId);

            return result;
        }

        public UserBoard GetUserBoardByCardId(int cardId, string userId)
        {
            var result = _userBoards
                .Include(x => x.Board)
                .ThenInclude(x => x.Lists)
                .ThenInclude(x => x.Cards)
                .FirstOrDefault(
                    b => b.UserId == userId
                         && !b.IsDeleted
                         && b.Board.Lists.Any(l => !l.IsDeleted
                                                   && l.Cards.Any(c => !c.IsDeleted
                                                                       && c.Id == cardId)));

            return result;
        }

        public UserBoard CreateUserBoard(UserBoard userBoard)
        {
            _userBoards.Add(userBoard);
            _context.SaveChanges();
            _context.Entry(userBoard).Reference(x => x.Board).Load();
            _context.Entry(userBoard.Board).Reference(x => x.Owner).Load();
            _context.Entry(userBoard).Reference(x => x.User).Load();
            return userBoard;
        }

        public void UpdateBoardTimeVisited(UserBoard userBoard)
        {
            userBoard.LastTimeVisited = DateTime.UtcNow;
            _context.Attach(userBoard);
            _context.Entry(userBoard).Property(x => x.LastTimeVisited).IsModified = true;
            _context.SaveChanges();
        }

        public ICollection<UserBoard> GetThreeLastVisitedBoards(string userId)
        {
            var result = _userBoards
                .Include(x => x.Board)
                .Include(x => x.Board.Members)
                .Include(x => x.Board.Owner)
                .Where(x => x.UserId == userId && !x.IsDeleted
                            && !x.Board.IsDeleted && x.LastTimeVisited > DateTime.MinValue)
                .OrderByDescending(x => x.LastTimeVisited)
                .Take(3)
                .ToList();

            return result;
        }

        public ICollection<UserBoard> GetUserBoards(string userId)
        {
            var result = _userBoards
                .Include(x => x.Board)
                .Include(x => x.Board.Members)
                .Include(x => x.Board.Owner)
                .Where(x => x.UserId == userId && !x.IsDeleted
                            && !x.Board.IsDeleted)
                .ToList();

            return result;
        }

        public UserBoard UpdatePriorityOfBoard(int boardId, string userId)
        {
            var userBoard = _userBoards.FirstOrDefault(x => x.BoardId == boardId && x.UserId == userId);
            if (userBoard != null)
            {
                userBoard.IsFavorite = !userBoard.IsFavorite;
                userBoard.LastTimeSetFavorite = DateTime.UtcNow;
                _context.SaveChanges();
            }
            return userBoard;
        }

        public ICollection<User> GetMembersOfBoard(int boardId)
        {
            var result = _userBoards.Include(x => x.User)
                    .Where(x => x.BoardId == boardId && !x.IsDeleted)
                    .Select(x => x.User)
                    .ToList();

            return result;
        }

        public bool IsMember(int boardId, string userId)
        {
            var result = _userBoards.Any(x => x.BoardId == boardId && !x.IsDeleted
                                              && !x.Board.IsDeleted
                                              && x.UserId == userId);

            return result;
        }

        public int GetNumberOfBoards(string userId)
        {
            var result = _userBoards.Count(x => x.UserId == userId
                                                && !x.IsDeleted
                                                && !x.Board.IsDeleted);

            return result;
        }

        public bool HasUserBoards(string userId)
        {
            var result = _userBoards.Any(x => x.UserId == userId
                                              && !x.IsDeleted
                                              && !x.Board.IsDeleted
                                              && x.LastTimeVisited > DateTime.MinValue);

            return result;
        }

        public int GetNumberOfCollaborators(string userId)
        {
            var userBoards = _userBoards.Include(x => x.Board)
                                    .ThenInclude(x => x.Members)
                                    .Where(x => x.UserId == userId && !x.IsDeleted
                                                && !x.Board.IsDeleted).SelectMany(x => x.Board.Members.Where(member => !member.IsDeleted));

            var result = userBoards.Select(x => x.User).ToHashSet().Count(x => x.Id != userId);
            return result;
        }

        public UserBoard ChangeUserBoardState(int boardId, string userId)
        {
            var userBoard = _userBoards
                .Include(x => x.Board)
                .Include(x => x.User)
                .FirstOrDefault(x => x.BoardId == boardId && x.UserId == userId);

            userBoard.LastTimeVisited = DateTime.MinValue;
            userBoard.IsDeleted = !userBoard.IsDeleted;
            _context.SaveChanges();
            return userBoard;
        }
    }
}
