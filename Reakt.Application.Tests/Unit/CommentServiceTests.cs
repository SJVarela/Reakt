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
    internal class CommentServiceTests
    {
        private readonly List<Comment> _mockData = new List<Comment>()
        {
            new Comment()
            {
                Id = 1,
                Message = "Test message",
                PostId = 1,
            }
        };

        private CommentService _commentService;

        private ReaktDbContext _context;

        private IMapper _mapper;

        [Test]
        public async Task Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<Domain.Models.Comment>(_mockData.First(b => b.Id == 1));
            //Act
            var result = await _commentService.GetAsync(1);

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Domain.Models.Comment>>(_mockData);
            //Act
            var result = await _commentService.GetAsync();

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
            _context.Comments.AddRange(_mockData);
            _context.SaveChanges();

            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new CommentProfile())));
            _commentService = new CommentService(_context, _mapper);
        }
    }
}