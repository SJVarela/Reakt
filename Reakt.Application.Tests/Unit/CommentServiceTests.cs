using AutoFixture;
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

        private Fixture _fixture;
        private IMapper _mapper;

        [Test]
        public void AddCommentAsync_Should_AddItem_Return_Results()
        {
            //Arrange
            var expected = _fixture.Build<DM.Comment>()
                                   .With(c => c.Id, 0)
                                   .Create();
            //Act
            var result = _commentService.AddCommentAsync(1, expected, null).Result;
            expected.CreatedAt = result.CreatedAt;
            expected.Id = result.Id;
            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void DeleteAsync_Should_UpdateDate_Active_To_False()
        {
            //Arrange
            var id = _context.Comments.First().Id;
            //Act
            _commentService.DeleteAsync(id, null).Wait();
            var expected = _context.Comments
                .IgnoreQueryFilters()
                .FirstOrDefault(c => c.Id == id);
            //Assert
            expected.Active.Should().Be(false);
            expected.DeletedAt.Should().NotBeNull();
        }

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new CommentProfile())));
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth
        }

        [Test]
        public void GetAsync_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<DM.Comment>>(_context.Comments.ToList());
            //Act
            var result = _commentService.GetAsync(null).Result;

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAsyncById_Should_Return_Results()
        {
            //Arrange
            var id = _context.Comments.First().Id;
            var expected = _mapper.Map<DM.Comment>(_context.Comments.First(x => x.Id == id));
            //Act
            var result = _commentService.GetAsync(id, null).Result;

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [SetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new CommentProfile())));
            _context = MockDbContextFactory.BuildInMemory(_mapper.Map<List<PM.Comment>>(_fixture.CreateMany<PM.Comment>(10)));
            _commentService = new CommentService(_context, _mapper);
        }
    }
}