using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    public class Board
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Post> Posts { get; set; }
    }
}
