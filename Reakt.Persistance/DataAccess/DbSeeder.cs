using Microsoft.EntityFrameworkCore;
using Reakt.Application.Persistence.Models;

namespace Reakt.Persistance.DataAccess
{
    public static class DbSeeder
    {
        public static void SeedDb(ModelBuilder builder)
        {
            builder.Entity<Board>().HasData(new Board
            {
                Id = 1,
                Title = "Seed bored",
                Description = "This is a seeded board"
            });

            builder.Entity<Post>().HasData(new Post
            {
                Id = 1,
                Title = "Seed post",
                Description = "This is a seeded post",
                BoardId = 1
            });

            builder.Entity<Comment>().HasData(new Comment
            {
                Id = 1,
                PostId = 1,
                Message = "This post sucks"
            },
            new Comment()
            {
                Id = 2,
                PostId = 1,
                Message = "You suck",
                ParentId = 1
            });
        }
    }
}