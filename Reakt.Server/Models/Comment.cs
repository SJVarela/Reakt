using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    /// <summary>
    /// A Reaktive post comment
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Comment identifier
        /// </summary>
        public long Id { get; private set; }
        /// <summary>
        /// Comment message
        /// </summary>
        [Required]
        [MaxLength(4000)]
        [MinLength(1)]
        public string Message { get; set; }
        /// <summary>
        /// Comment this replies to
        /// </summary>
        public Comment Parent { get; set; }
        /// <summary>
        /// Date the comment was created
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        /// <summary>
        /// Date the comment was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }
        /// <summary>
        /// Likes ammount
        /// </summary>
        public int Likes { get; private set; }


    }
}
