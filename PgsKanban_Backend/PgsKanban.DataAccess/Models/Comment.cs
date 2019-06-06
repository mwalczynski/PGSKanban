using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PgsKanban.DataAccess.Interfaces;

namespace PgsKanban.DataAccess.Models
{
    public class Comment : IDbEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CardId { get; set; }
        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public string ExternalUserId { get; set; }
        [ForeignKey("ExternalUserId")]
        public virtual ExternalUser ExternalUser { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
