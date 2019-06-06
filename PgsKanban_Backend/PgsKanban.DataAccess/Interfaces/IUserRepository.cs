using System;
using System.Collections.Generic;
using System.Text;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByFullName(string firstName, string lastName);
        ExternalUser GetExternalUserByFullName(string firstName, string lastName);
        ExternalUser AddExternalUser(ExternalUser externalUser);
    }
}
