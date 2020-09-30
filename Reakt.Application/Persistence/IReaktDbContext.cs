using Microsoft.EntityFrameworkCore;
using Reakt.Application.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Application.Persistence
{
    public interface IReaktDbContext
    {
        DbSet<Comment> Comments { get; set; }
        DbSet<Post> Posts { get; set; }
        public DbSet<Board> Boards { get; set; }
    }
}
