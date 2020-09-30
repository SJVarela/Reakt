using System.Collections.Generic;

namespace Reakt.Application.Persistence.Models
{
    public class Board : AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
