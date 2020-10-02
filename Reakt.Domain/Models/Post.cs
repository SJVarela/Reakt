using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Domain.Models
{
    public class Post
    {
        public long Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long BoardId { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
