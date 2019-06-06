using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PgsKanban.Import.Dtos
{
    public class ImportedActionDto
    {
        public JObject Data { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public ImportedMemberDto MemberCreator { get; set; }
    }
}
