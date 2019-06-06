using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class EditCardNameDto
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
    }
}
