using Reakt.Application.Persistence.Attributes;
using System.Collections.Generic;

namespace Reakt.Application.Persistence.Models
{
    public class Comment : AuditableEntity
    {
        public int Likes { get; set; }

        [Tracked]
        public string Message { get; set; }

        public long? ParentId { get; set; }
        public long PostId { get; set; }
        public virtual IEnumerable<Comment> Replies { get; set; }
        public int ReplyCount { get; set; }
    }
}