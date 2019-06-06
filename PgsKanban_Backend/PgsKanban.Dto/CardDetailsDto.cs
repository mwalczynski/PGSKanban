using System;
using System.Collections.Generic;
using System.Text;

namespace PgsKanban.Dto
{
    public class CardDetailsDto
    {
        public int Id { get; set; }
        public string ObfuscatedId { get; set; }
        public string Name { get; set; }
        public string ListName { get; set; }
        public string Description { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}
