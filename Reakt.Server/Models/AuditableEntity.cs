﻿using System;

namespace Reakt.Server.Models
{
    /// <summary>
    /// Enitity that tracks modification
    /// </summary>
    public class AuditableEntity : BaseEntity
    {
        /// <summary>
        /// Date the entity was created
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Date the entity was deleted
        /// </summary>
        public DateTime? DeletedAt { get; private set; }

        /// <summary>
        /// Date the entity was updated
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }
    }
}