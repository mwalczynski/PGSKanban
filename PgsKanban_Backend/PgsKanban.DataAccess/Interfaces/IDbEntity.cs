namespace PgsKanban.DataAccess.Interfaces
{
    public interface IDbEntity
    {
        bool IsDeleted { get; set; }
    }
}
