using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class AddBoardDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
