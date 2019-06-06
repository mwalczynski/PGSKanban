using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class CommentCardDto
    {
        [Required]
        public int CardId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
