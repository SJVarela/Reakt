using Microsoft.EntityFrameworkCore;
using Reakt.Application.Persistence;
using Reakt.Persistance.DataAccess;
using System;
using System.Collections.Generic;

namespace Reakt.Application.Tests.MockFactories
{
    public static class MockDbContextFactory
    {
        public static ReaktDbContext BuildInMemory<T>(IEnumerable<T> seedData) where T : BaseEntity
        {
            var context = new ReaktDbContext(
                new DbContextOptionsBuilder<ReaktDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            context.Set<T>().AddRange(seedData);
            context.SaveChanges();
            return context;
        }
    }
}