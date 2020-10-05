using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Server.Models
{
    /// <summary>
    /// Base tracked entity
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Enitity identifier
        /// </summary>
        public long Id { get; set; }
    }
}
