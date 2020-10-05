using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Domain.Models
{
    public class Board : AuditableEntity
    {        
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Post> Posts { get; set; }
    }
}
