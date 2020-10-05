using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Reakt.Application.Persistence.Models;
using Reakt.Application.Services;
using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    internal class PostServiceTests
    {
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

        private ReaktDbContext _context;
        private IMapper _mapper;
        private PostService _postService;

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
    }
}