using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class DeleteMemberDto
    {
        public string MemberId { get; set; }
        public int BoardId { get; set; }
    }
}
