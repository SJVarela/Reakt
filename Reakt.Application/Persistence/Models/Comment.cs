using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Persistence.Models
{
    public class Comment : AuditableEntity
    {
        public long PostId { get; set; }
        public string Message { get; set; }
        public long? ParentId { get; set; }
        public virtual Comment Parent { get; set; }
        public int Likes { get; set; }
    }
}
