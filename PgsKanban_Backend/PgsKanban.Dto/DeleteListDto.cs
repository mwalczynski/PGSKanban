using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class DeleteListDto
    {
        public int ListId { get; set; }
        public int BoardId { get; set; }
    }
}
