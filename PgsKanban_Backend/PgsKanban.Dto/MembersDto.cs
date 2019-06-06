using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class MembersDto
    {
        public ICollection<MemberDto> Members { get; set; }
        public bool IsAllowedToModify { get; set; }
    }
}
