using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    public class Post
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
