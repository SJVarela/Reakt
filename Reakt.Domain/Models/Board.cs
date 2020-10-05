using System.Collections.Generic;

namespace Reakt.Domain.Models
{
    public class Board : AuditableEntity
    {
        public string Description { get; set; }
        public List<Post> Posts { get; set; }
        public string Title { get; set; }
    }
}