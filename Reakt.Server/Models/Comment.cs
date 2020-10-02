using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.Models
{
    public class Comment
    {
        public long Id { get; private set; }
        [Required]
        [MaxLength(4000)]
        [MinLength(1)]
        public string Message { get; set; }
        public Comment Parent { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public int Likes { get; private set; }


    }
}
