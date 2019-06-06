namespace PgsKanban.Dto
{
    public class EditCardPositionDto
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public int NewListId { get; set; }
        public int NewPosition { get; set; }
    }
}
