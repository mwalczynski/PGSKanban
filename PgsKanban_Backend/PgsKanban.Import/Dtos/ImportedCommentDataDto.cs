using System;
using System.Collections.Generic;
using System.Text;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.Import.Dtos
{
    public class ImportedCommentDataDto
    {
        public Comment Comment { get; set; }
        public string ImportedCardId { get; set; }
    }
}
