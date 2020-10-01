using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public Comment Parent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public int Likes { get; set; }
    }
}
