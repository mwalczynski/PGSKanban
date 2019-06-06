namespace PgsKanban.Dto
{
    public class CardDto
    {
        public int Id { get; set; }
        public string ObfuscatedId { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int ListId { get; set; }
    }
}
