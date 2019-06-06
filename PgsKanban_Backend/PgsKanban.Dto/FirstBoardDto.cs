using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class FirstBoardDto
    {
        [Required, MaxLength(100)]
        public string BoardName { get; set; }
        [MaxLength(100)]
        public string ListName { get; set; }
        [MaxLength(100)]
        public string CardName { get; set; }
    }
}
