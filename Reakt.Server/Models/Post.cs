using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    public class Post
    {
        public long Id { get; private set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public List<Comment> Comments { get; set; }
    }
}
