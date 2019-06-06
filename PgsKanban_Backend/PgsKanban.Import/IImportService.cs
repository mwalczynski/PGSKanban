using PgsKanban.Import.Dtos;

namespace PgsKanban.Import
{
    public interface IImportService
    {
        ImportStatisticsDto ImportBoard(FileFormDto data, string userId);
    }
}
