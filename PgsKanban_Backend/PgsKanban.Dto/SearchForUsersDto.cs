using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class SearchForUsersDto
    {
        [Required, MinLength(3)]
        public string SearchPhrase { get; set; }
        [Required]
        public int BoardId { get; set; }
    }
}
