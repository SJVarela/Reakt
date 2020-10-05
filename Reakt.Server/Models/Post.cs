using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    /// <summary>
    /// A Reaktive post
    /// </summary>
    public class Post : AuditableEntity
    {
        /// <summary>
        /// Post title
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// Post description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Post comments
        /// </summary>
        public List<Comment> Comments { get; set; }
    }
}
