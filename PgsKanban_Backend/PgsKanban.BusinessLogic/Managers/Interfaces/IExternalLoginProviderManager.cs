using System;
using System.Collections.Generic;
using System.Text;
using PgsKanban.BusinessLogic.Enums;

namespace PgsKanban.BusinessLogic.Managers
{
    public interface IExternalLoginProviderManager
    {
        bool IsProviderAvailable(ExternalLoginProvider loginProvider);
    }
}
