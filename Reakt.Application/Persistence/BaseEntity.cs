using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Persistence
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public bool Active { get; set; }
    }
}
