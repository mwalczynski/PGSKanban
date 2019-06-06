using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class EditBoardDto
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
