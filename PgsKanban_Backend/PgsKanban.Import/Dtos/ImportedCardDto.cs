using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PgsKanban.Import.Dtos
{
    public class ImportedCardDto
    {
        public string Id { get; set; }
        [JsonProperty("idList")]
        public string ListId { get; set; }
        public string Name { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
    }
}
