using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Domain.Models
{
    public class Post
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
