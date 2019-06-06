using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PgsKanban.DataAccess.Interfaces;

namespace PgsKanban.DataAccess.Models
{
    public class UserBoard : IDbEntity
    {
        [Column(Order = 0), Key, ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Column(Order = 1), Key, ForeignKey("Boards")]
        public int BoardId { get; set; }
        [ForeignKey("BoardId")]
        public virtual Board Board { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime LastTimeVisited { get; set; }
        public DateTime LastTimeSetFavorite { get; set; }
        public bool IsDeleted { get; set; }
    }
}
