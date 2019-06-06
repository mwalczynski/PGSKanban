using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Implementation
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        private readonly DbSet<Comment> _comments;


        public CommentRepository(PgsKanbanContext context) : base(context)
        {
            _comments = context.Comments;
        }

        public Comment CreateComment(Comment comment)
        {
            comment.TimeCreated = DateTime.UtcNow;
            _comments.Add(comment);
            _context.SaveChanges();
            _context.Entry(comment).Reference(x => x.User).Load();
            return comment;
        }
    }
}
