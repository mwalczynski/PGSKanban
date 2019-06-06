using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using PgsKanban.BusinessLogic.Enums;
using PgsKanban.BusinessLogic.Interfaces;
using PgsKanban.BusinessLogic.Options;

namespace PgsKanban.BusinessLogic.Managers
{
    public class ExternalLoginProviderManager: IExternalLoginProviderManager
    {
        private readonly AvailableExternalProvidersOptions _availableProviderOptions;
        public ExternalLoginProviderManager(IOptions<AvailableExternalProvidersOptions> availableProviderOptionsAccessor)
        {
            _availableProviderOptions = availableProviderOptionsAccessor.Value;
        }
        public bool IsProviderAvailable(ExternalLoginProvider loginProvider)
        {
            return _availableProviderOptions.Providers.Any(provider => provider == loginProvider);
        }

    }
}
