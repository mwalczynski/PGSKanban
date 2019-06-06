using System.Collections.Generic;

namespace PgsKanban.Dto
{
    public class ListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int BoardId { get; set; }
        public ICollection<CardDto> Cards { get; set; }
    }
}
