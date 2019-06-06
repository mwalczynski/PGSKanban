using System.Collections.Generic;

namespace PgsKanban.Import.Dtos
{
    public class ImportedBoardDto
    {
        public string Name { get; set; }
        public IList<ImportedListDto> Lists { get; set; }
        public IList<ImportedCardDto> Cards { get; set; }
        public IList<ImportedActionDto> Actions { get; set; }
    }
}
