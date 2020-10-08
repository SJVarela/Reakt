using Reakt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reakt.Application.Tests.MockFactories
{
    internal class EntityFactory<T> where T : BaseEntity, new()
    {
        public virtual T BuildMock(long id)
        {
            var mock = new T
            {
                Id = id
            };
            mock.GetType().GetProperties(BindingFlags.Public)
                .Where(x => x.PropertyType == typeof(string))
                .ToList()
                .ForEach(x => x.SetValue(mock, Guid.NewGuid().ToString()));
            return mock;
        }

        public List<T> BuildMockList(long startId, long endId)
        {
            var list = new List<T>();
            for (long i = startId; i <= endId; i++)
            {
                list.Add(BuildMock(i));
            }
            return list;
        }
    }
}