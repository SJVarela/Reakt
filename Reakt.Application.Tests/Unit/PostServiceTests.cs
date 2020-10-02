using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Reakt.Application.Persistence;
using Reakt.Application.Persistence.Models;
using Reakt.Application.Services;
using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    class PostServiceTests
    {
        private ReaktDbContext _context;
        private IMapper _mapper;
        private PostService _postService;

        private readonly List<Post> _mockData = new List<Post>()
        {
            new Post()
            {
                Id = 1,
                Title = "Test title",
                Description = "Test desc",
                BoardId = 1,
                Comments = new List<Comment>()
            }
        };

        [OneTimeSetUp]
        public void Setup()
        {
            //setup inmemorydb
            var x = new DbContextOptionsBuilder<ReaktDbContext>();
            x.UseInMemoryDatabase("UtDb");
            _context = new ReaktDbContext(x.Options);
            _context.Posts.AddRange(_mockData);
            _context.SaveChanges();

            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new PostProfile())));
            _postService = new PostService(_context, _mapper);
        }

        [Test]
        public async Task Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Domain.Models.Post>>(_mockData);
            //Act
            var result = await _postService.GetAsync();

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public async Task Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<Domain.Models.Post>(_mockData.First(b => b.Id == 1));
            //Act
            var result = await _postService.GetAsync(1);

            //Arrange            
            result.Should().BeEquivalentTo(expected);
        }
    }
}
