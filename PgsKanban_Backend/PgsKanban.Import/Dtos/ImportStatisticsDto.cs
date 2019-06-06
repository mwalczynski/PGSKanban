using System;
using System.Collections.Generic;
using System.Text;
using PgsKanban.Dto;

namespace PgsKanban.Import.Dtos
{
    public class ImportStatisticsDto
    {
        public int ListsCount { get; set; }
        public int CardsCount { get; set; }
        public int CommentsCount { get; set; }
        public UserBoardDto UserBoard { get; set; }
    }
}
