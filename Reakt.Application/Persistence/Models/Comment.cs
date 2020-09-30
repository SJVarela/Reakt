using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Persistence.Models
{
    public class Comment
    {
        public ulong Id { get; set; }
        public string Message { get; set; }
        public int ParentId { get; set; }
        public virtual Comment Parent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public int Likes { get; set; }
    }
}
