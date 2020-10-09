using System.Collections.Generic;

namespace Reakt.Application.Persistence.Models
{
    public class Board : AuditableEntity
    {
        public string Description { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }
        public string Title { get; set; }
    }
}