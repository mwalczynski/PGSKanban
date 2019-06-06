using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PgsKanban.DataAccess.Interfaces;

namespace PgsKanban.DataAccess.Models
{
    public class List : IDbEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int Position { get; set; }
        [Required]
        public int BoardId { get; set; }
        [ForeignKey("BoardId")]
        public virtual Board Board { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public bool IsDeleted { get; set; }
    }
}
