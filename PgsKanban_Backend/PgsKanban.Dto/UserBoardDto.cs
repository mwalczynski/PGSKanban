using System;

namespace PgsKanban.Dto
{
    public class UserBoardDto
    {
        public string UserId { get; set; }
        public int BoardId { get; set; }
        public bool IsOwner { get; set; }
        public bool IsFavorite { get; set; }
        public BoardMiniatureDto Board { get; set; }
        public DateTime LastTimeVisited { get; set; }
        public DateTime LastTimeSetFavorite { get; set; }
    }
}
