
namespace PgsKanban.Dto
{
    public class BoardMiniatureDto
    {
        public int Id { get; set; }
        public string ObfuscatedId { get; set; }
        public string Name { get; set; }
        public UserProfileDto Owner { get; set; }
        public int MembersCount { get; set; }
    }
}
