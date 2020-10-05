using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    /// <summary>
    /// A Reaktive message board
    /// </summary>
    public class Board : AuditableEntity
    {
        /// <summary>
        /// Board Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Board description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Board posts
        /// </summary>
        public List<Post> Posts { get; set; }
    }
}
