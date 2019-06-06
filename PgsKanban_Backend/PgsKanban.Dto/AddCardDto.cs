using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class AddCardDto
    {
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required]
        public int ListId { get; set; }
    }
}
