﻿using System.Collections.Generic;

namespace Reakt.Application.Persistence.Models
{
    public class Post : AuditableEntity
    {
        public long BoardId { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}