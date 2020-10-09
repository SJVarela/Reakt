using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reakt.Server.Models
{
    /// <summary>
    /// A Reaktive board post
    /// </summary>
    public class Post : AuditableEntity
    {
        /// <summary>
        /// Post comments
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// Post description
        /// </summary>
        [MaxLength(4000)]
        [MinLength(1)]
        public string Description { get; set; }

        /// <summary>
        /// Post Title
        /// </summary>
        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string Title { get; set; }
    }
}