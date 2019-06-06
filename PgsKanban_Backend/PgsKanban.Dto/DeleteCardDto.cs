using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class DeleteCardDto
    {
        public int CardId { get; set; }
        public int ListId { get; set; }
    }
}
