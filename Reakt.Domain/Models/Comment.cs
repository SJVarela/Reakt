using System.Collections.Generic;

namespace Reakt.Domain.Models
{
    public class Comment : AuditableEntity
    {
        public int Likes { get; set; }
        public string Message { get; set; }
        public IEnumerable<Comment> Replies { get; set; }
        public int ReplyCount { get; set; }
    }
}