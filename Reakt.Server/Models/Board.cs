using System.Collections.Generic;

namespace Reakt.Server.Models
{
    /// <summary>
    /// A Reaktive message board
    /// </summary>
    public class Board : AuditableEntity
    {
        /// <summary>
        /// Board description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Board posts
        /// </summary>
        public IEnumerable<Post> Posts { get; set; }

        /// <summary>
        /// Board Title
        /// </summary>
        public string Title { get; set; }
    }
}