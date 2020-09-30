using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Persistence.Models
{
    public class Post : AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
