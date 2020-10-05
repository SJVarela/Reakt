using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Server.Models
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Enitity identifier
        /// </summary>
        public long Id { get; set; }
    }
}
