﻿using System.Collections.Generic;

namespace Reakt.Domain.Models
{
    public class Post : AuditableEntity
    {
        public long BoardId { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}