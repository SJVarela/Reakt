using Newtonsoft.Json;
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
        /// Comment this replies to
        /// </summary>
        public Comment Parent { get; set; }
    }
}