using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.BusinessLogic.Services
{
    public static class TokenGenerator
    {
        public static string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
