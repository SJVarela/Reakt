using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Reakt.Application.Services;
using Reakt.Application.Tests.MockFactories;
using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;
using System.Collections.Generic;
using System.Linq;
using DM = Reakt.Domain.Models;
using PM = Reakt.Application.Persistence.Models;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    internal class CommentServiceTests
    {
        private CommentService _commentService;

        private ReaktDbContext _context;

        private EntityFactory<DM.Comment> _entityFactory = new EntityFactory<DM.Comment>();
        private IMapper _mapper;

        [Test]
        public void AddCommentAsync_Should_AddItem_Return_Results()
        {
            //Arrange
            var expected = _entityFactory.BuildMock(5);
            //Act
            var result = _commentService.AddCommentAsync(1, expected).Result;
            expected.CreatedAt = result.CreatedAt;
            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_Should_UpdateDate_Active_To_False()
        {
            //Arrange

            //Act
            _commentService.DeleteAsync(1).Wait();
            var expected = _context.Comments
                .IgnoreQueryFilters()
                .FirstOrDefault(c => c.Id == 1);
            //Assert
            expected.Active.Should().Be(false);
            expected.DeletedAt.Should().NotBeNull();
        }

        [Test]
        public void GetAsync_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<DM.Comment>>(_context.Comments.ToList());
            //Act
            var result = _commentService.GetAsync().Result;

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAsyncById_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<DM.Comment>(_context.Comments.First(x => x.Id == 1));
            //Act
            var result = _commentService.GetAsync(1).Result;

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [SetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new CommentProfile())));
            _context = MockDbContextFactory.BuildInMemory(_mapper.Map<List<PM.Comment>>(_entityFactory.BuildMockList(1, 4)));
            _commentService = new CommentService(_context, _mapper);
        }
    }
}