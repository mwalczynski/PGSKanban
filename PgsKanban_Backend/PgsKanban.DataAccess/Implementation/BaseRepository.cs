namespace PgsKanban.DataAccess.Implementation
{
    public abstract class BaseRepository
    {
        protected readonly PgsKanbanContext _context;

        protected BaseRepository(PgsKanbanContext context)
        {
            _context = context;
        }
    }
}
