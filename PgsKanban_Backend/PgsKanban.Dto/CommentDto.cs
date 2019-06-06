using System;

namespace PgsKanban.Dto
{
    public class CommentDto
    {
        public string Content { get; set; }
        public bool IsOwner { get; set; }
        public DateTime TimeCreated { get; set; }
        public UserProfileDto User { get; set; }
        public UserProfileDto ExternalUser { get; set; }
    }
}
