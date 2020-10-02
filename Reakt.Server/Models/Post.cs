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
    public class Post
    {
        /// <summary>
        /// Post identifier
        /// </summary>
        public long Id { get; private set; }
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
        /// Date the post was created
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        /// <summary>
        /// Date the post was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }
        /// <summary>
        /// Post comments
        /// </summary>
        public List<Comment> Comments { get; set; }
    }
}
