using System;
using System.Collections.Generic;
using System.Text;
using PgsKanban.BusinessLogic.Enums;

namespace PgsKanban.BusinessLogic.Options
{
    public class AvailableExternalProvidersOptions
    {
        public IEnumerable<ExternalLoginProvider> Providers { get; set; }
    }
}
