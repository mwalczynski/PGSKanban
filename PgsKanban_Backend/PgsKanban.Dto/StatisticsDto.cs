using System.Collections.Generic;

namespace PgsKanban.Dto
{
    public class StatisticsDto
    {
        public int Boards { get; set; }
        public int Lists { get; set; }
        public int Cards { get; set; }
        public int Collaborators { get; set; }
        public ICollection<UserBoardDto> LatestUserBoards { get; set; }
    }
}
