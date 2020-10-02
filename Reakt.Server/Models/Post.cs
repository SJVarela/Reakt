using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    /// <summary>
    /// A Reaktive board post
    /// </summary>
    public class Post
    {
        public long Id { get; private set; }
        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string Title { get; set; }
        [MaxLength(4000)]
        [MinLength(1)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public List<Comment> Comments { get; set; }
    }
}
