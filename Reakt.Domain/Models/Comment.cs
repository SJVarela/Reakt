using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Domain.Models
{
    public class Comment : AuditableEntity
    {
        public string Message { get; set; }
        public Comment Parent { get; set; }
        public int Likes { get; set; }
    }
}
