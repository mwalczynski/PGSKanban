using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PgsKanban.BusinessLogic.OpenIdHelpers
{
    public class JwksKey
    {
        [JsonProperty("e")]
        public string KeyExponent { get; set; }
        [JsonProperty("n")]
        public string KeyModules { get; set; }
        public string Kid { get; set; }

    }
}
