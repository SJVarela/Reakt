using Microsoft.EntityFrameworkCore;
using Reakt.Application.Persistence;
using Reakt.Application.Persistence.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reakt.Persistance.DataAccess
{
    public class ReaktDbContext : DbContext, IReaktDbContext
    {
        private void UpdateAuditableState()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.Active = true;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.DeletedAt = DateTime.Now;
                        entry.Entity.Active = false;
                        entry.State = EntityState.Modified;
                        break;

                    default:
                        continue;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReaktDbContext).Assembly);
            DbSeeder.SeedDb(modelBuilder);
        }

        public ReaktDbContext(DbContextOptions<ReaktDbContext> options) : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }

        public override int SaveChanges()
        {
            UpdateAuditableState();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditableState();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}