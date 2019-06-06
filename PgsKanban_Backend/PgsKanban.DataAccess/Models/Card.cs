using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PgsKanban.DataAccess.Interfaces;

namespace PgsKanban.DataAccess.Models
{
    public class Card : IDbEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Position { get; set; }
        [Required]
        public int ListId { get; set; }
        [ForeignKey("ListId")]
        public virtual List List { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public bool IsDeleted { get; set; }
    }
}
