﻿using Microsoft.EntityFrameworkCore;
using Reakt.Application.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Persistence
{
    public interface IReaktDbContext
    {
        DbSet<Comment> Comments { get; set; }
        DbSet<Post> Posts { get; set; }
        public DbSet<Board> Boards { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
