using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PgsKanban.Dto;

namespace PgsKanban.Hubs.Interfaces
{
    public interface ICardDetailsClientHandler
    {
        Task ChangedLongDescription(string newDescription);
        Task ChangedName(string newName);
        Task AddComment(CommentDto comment);
    }
}
