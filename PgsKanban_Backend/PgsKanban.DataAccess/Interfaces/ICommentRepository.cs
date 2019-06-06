using System;
using System.Collections.Generic;
using System.Text;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Interfaces
{
    public interface ICommentRepository
    {
        Comment CreateComment(Comment comment);
    }
}
