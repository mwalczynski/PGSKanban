using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class AddListDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int BoardId { get; set; }
    }
}
