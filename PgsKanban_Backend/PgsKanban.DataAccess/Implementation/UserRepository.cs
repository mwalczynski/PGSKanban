using System.Linq;
using PgsKanban.DataAccess.Interfaces;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Implementation
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(PgsKanbanContext context) : base(context)
        {
        }

        public User GetUserByFullName(string firstName, string lastName)
        {
            return _context.Users.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
        }

        public ExternalUser GetExternalUserByFullName(string firstName, string lastName)
        {
            return _context.ExternalUsers.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
        }

        public ExternalUser AddExternalUser(ExternalUser externalUser)
        {
            _context.ExternalUsers.Add(externalUser);
            _context.SaveChanges();
            return externalUser;
        }
    }
}
