using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Implementation
{
    public class ListRepository : BaseRepository, IListRepository
    {
        private readonly DbSet<List> _lists;

        public ListRepository(PgsKanbanContext context) : base(context)
        {
            _lists = context.Lists;
        }

        public List CreateList(List list)
        {
            _lists.Add(list);
            _context.SaveChanges();
            return list;
        }

        public List GetList(int id)
        {
            var result = _lists.FirstOrDefault(x => x.Id == id);

            return result;
        }

        public List UpdateListName(List list)
        {
            _context.Attach(list);
            _context.Entry(list).Property(x => x.Name).IsModified = true;
            _context.SaveChanges();
            return list;
        }

        public bool IsOwner(int listId, string userId)
        {
            var result = _lists.Any(x => x.Id == listId
                                         && !x.IsDeleted
                                         && x.Board.IsDeleted == false
                                         && x.Board.OwnerId == userId);

            return result;
        }

        public bool IsMember(int listId, string userId)
        {
            var result = _lists.Any(x => x.Id == listId
                                         && !x.IsDeleted
                                         && x.Board.IsDeleted == false
                                         && x.Board.Members.Any(y => y.UserId == userId));

            return result;
        }

        public int GetNumberOfLists(string userId)
        {
            var result = _lists.Count(x => !x.Board.IsDeleted && !x.IsDeleted
                                           && x.Board.Members.Any(y => y.UserId == userId && !y.IsDeleted));
            return result;
        }

        public ICollection<List> GetListsInPositionRange(int boardId, int startIndex, int endIndex)
        {
            var result = _lists.Where(x => x.BoardId == boardId
                                           && !x.IsDeleted
                                           && x.Position > startIndex
                                           && x.Position < endIndex)
                .ToList();

            return result;
        }

        public void UpdateListsPosition(ICollection<List> lists)
        {
            foreach (var list in lists)
            {
                _context.Attach(list);
                _context.Entry(list).Property(x => x.Position).IsModified = true;
            }
            _context.SaveChanges();
        }

        public int GetNumberOfListsInBoard(int boardId)
        {
            var result = _lists.Count(x => x.BoardId == boardId && !x.IsDeleted);
            return result;
        }

        public List DeleteList(List list)
        {
            list.IsDeleted = true;
            _context.SaveChanges();
            return list;
        }

        public List<List> GetListsWithGreaterPosition(int boardId, int position)
        {
            var result = _lists.Where(x => x.BoardId == boardId
                                           && !x.IsDeleted
                                           && x.Position > position)
                .ToList();

            return result;
        }
    }
}
