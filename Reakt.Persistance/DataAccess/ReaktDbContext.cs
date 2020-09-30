using Microsoft.EntityFrameworkCore;
using Reakt.Application.Persistence;
using Reakt.Application.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reakt.Persistance.DataAccess
{
    public class ReaktDbContext : DbContext, IReaktDbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Board> Boards { get; set; }
    }
}
