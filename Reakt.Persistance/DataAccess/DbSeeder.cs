using Microsoft.EntityFrameworkCore;
using Reakt.Application.Persistence.Models;
using System;

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
                Description = "This is a seeded board",
                Active = true
            });

            builder.Entity<Post>().HasData(new Post
            {
                Id = 1,
                Title = "Seed post",
                Description = "This is a seeded post",
                BoardId = 1,
                Active = true
            });

            builder.Entity<Comment>().HasData(new Comment
            {
                Id = 1,
                PostId = 1,
                Message = "This post sucks",
                ParentId = null,
                Active = true,
                CreatedAt = DateTime.Now
            },
            new Comment
            {
                Id = 2,
                PostId = 1,
                Message = "This post is good",
                ParentId = null,
                Active = true,
                CreatedAt = DateTime.Now,
            },
            new Comment
            {
                Id = 3,
                PostId = 1,
                Message = "This comment is good",
                ParentId = 1,
                Active = true,
                CreatedAt = DateTime.Now
            });
        }
    }
}