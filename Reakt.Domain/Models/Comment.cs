using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Domain.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public Comment Parent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Likes { get; set; }
    }
}
