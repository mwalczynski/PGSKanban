using System.Collections.Generic;

namespace PgsKanban.Dto
{
    public class BoardDto
    {
        public int Id { get; set; }
        public string ObfuscatedId { get; set; }
        public string Name { get; set; }
        public ICollection<ListDto> Lists { get; set; }
        public bool IsOwner { get; set; }
    }
}
