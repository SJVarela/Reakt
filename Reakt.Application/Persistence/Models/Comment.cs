using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Persistence.Models
{
    public class Comment : AuditableEntity
    {        
        public string Message { get; set; }
        public ulong? ParentId { get; set; }
        public virtual Comment Parent { get; set; }        
        public int Likes { get; set; }
    }
}
