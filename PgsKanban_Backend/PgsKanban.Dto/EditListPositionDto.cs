namespace PgsKanban.Dto
{
    public class EditListPositionDto
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public int NewPosition { get; set; }
    }
}
