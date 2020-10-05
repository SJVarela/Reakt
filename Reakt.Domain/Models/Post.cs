using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Domain.Models
{
    public class Post
    {
        public long BoardId { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}