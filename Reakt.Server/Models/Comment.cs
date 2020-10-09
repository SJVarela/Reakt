using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reakt.Server.Models
{
    /// <summary>
    /// A Reaktive post comment
    /// </summary>
    public class Comment : AuditableEntity
    {
        /// <summary>
        /// Likes ammount
        /// </summary>
        public int Likes { get; private set; }

        /// <summary>
        /// Comment message
        /// </summary>
        [Required]
        [MaxLength(4000)]
        [MinLength(1)]
        public string Message { get; set; }

        /// <summary>
        /// Replies to this comment
        /// </summary>
        public IEnumerable<Comment> Replies { get; set; }

        /// <summary>
        /// Number of replies to this comment
        /// </summary>
        public int ReplyCount { get; set; }
    }
}