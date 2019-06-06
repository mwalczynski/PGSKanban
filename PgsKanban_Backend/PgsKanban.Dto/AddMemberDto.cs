using System.ComponentModel.DataAnnotations;

namespace PgsKanban.Dto
{
    public class AddMemberDto
    {
        [Required]
        public string MemberId { get; set; }
        [Required]
        public int BoardId { get; set; }
    }
}
