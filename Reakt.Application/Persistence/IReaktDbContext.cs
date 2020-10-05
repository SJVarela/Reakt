using Microsoft.EntityFrameworkCore;
using Reakt.Application.Persistence.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Application.Persistence
{
    public interface IReaktDbContext
    {
        DbSet<Board> Boards { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Post> Posts { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}