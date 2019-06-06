using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Import.Dtos
{
    public class ImportedCommentDto
    {
        public ImportedCardDto Card { get; set; }
        public string Text { get; set; }
    }
}
