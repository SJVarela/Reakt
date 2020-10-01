using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Persistence
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }        
    }
}
