using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PgsKanban.DataAccess.Interfaces;

namespace PgsKanban.DataAccess.Models
{
    public class Board : IDbEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Hash { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public User Owner { get; set; }
        public virtual ICollection<UserBoard> Members { get; set; }
        public virtual ICollection<List> Lists { get; set; }
        public bool IsDeleted { get; set; }
    }
}
