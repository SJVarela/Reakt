using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Reakt.Application.Services;
using Reakt.Application.Tests.MockFactories;

using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using DM = Reakt.Domain.Models;
using PM = Reakt.Application.Persistence.Models;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    public class PostServiceTests
    {
        private ReaktDbContext _context;
        private Fixture _fixture;
        private IMapper _mapper;
        private PostService _postService;

        [Test]
        public void AddAsync_Should_Add_Post_To_DbContext()
        {
            //Arrange
            var boardId = 1;
            var post = _fixture.Build<DM.Post>()
                               .With(p => p.Id, 0)
                               .Without(p => p.Comments)
                               .Create();

            //Act
            var result = _postService.AddAsync(boardId, post).Result;

            //Assert
            result.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public void AddAsync_Should_Set_CreatedAt_Property()
        {
            //Arrange
            var boardId = 1;
            var post = _fixture.Build<DM.Post>()
                               .With(p => p.Id, 0)
                               .Without(p => p.Comments)
                               .Create();

            //Act
            var result = _postService.AddAsync(boardId, post).Result;

            //Assert
            result.CreatedAt.Should().NotBeSameDateAs(new DateTime());
        }

        [Test]
        public void CreateAsync_Should_Throw_Exception()
        {
            //Arrange
            var post = _fixture.Build<DM.Post>()
                   .With(p => p.Id, 0)
                   .Create();

            //Act-Assert
            _postService.Invoking(x => x.CreateAsync(post)).Should().Throw<NotImplementedException>();
        }

        [Test]
        public void Delete_Should_Mark_Items_Inactive()
        {
            //Arrange
            var id = _context.Posts.First().Id;

            //Act
            _postService.DeleteAsync(id).Wait();
            var result = _context.Posts.FirstOrDefault(x => x.Id == id);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void Delete_Should_Populate_DeletedAt_Property()
        {
            //Arrange
            var id = _context.Posts.First().Id;

            //Act
            _postService.DeleteAsync(id).Wait();
            var deletedAt = _context.Posts.IgnoreQueryFilters().First(p => p.Id == id).DeletedAt;

            //Assert
            deletedAt.Should().NotBeNull();
        }

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new PostProfile())));
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                             .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth
        }

        [Test]
        public void Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<DM.Post>>(_context.Posts.ToList());
            //Act
            var result = _postService.GetAsync().Result;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAsyncById_Should_Return_Null_When_Id_Not_Found()
        {
            //Arrange
            var invalidId = _context.Posts.Max(p => p.Id) + 1;

            //Act
            var result = _postService.GetAsync(invalidId).Result;

            //Arrange
            result.Should().BeNull();
        }

        [Test]
        public void GetAsyncById_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<DM.Post>(_context.Posts.First());
            //Act
            var result = _postService.GetAsync(expected.Id).Result;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetForBoard_Should_Return_Results()
        {
            //Arrange
            var boardId = 1;
            var expected = _mapper.Map<List<DM.Post>>(_context.Posts);

            //Act
            var result = _postService.GetForBoardAsync(boardId, 0, 50).Result;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [SetUp]
        public void Setup()
        {
            //setup inmemorydb
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new PostProfile())));
            _context = MockDbContextFactory.BuildInMemory(_mapper.Map<List<PM.Post>>(_fixture.Build<PM.Post>().Without(x => x.Comments).CreateMany(10)));
            _postService = new PostService(_context, _mapper);
        }

        [Test]
        public void Update_Should_Actualize_UpdatedAt_Property()
        {
            //Arrange
            var newTitle = "Modified Title";
            var expected = _mapper.Map<DM.Post>(_context.Posts.First());
            expected.Title = newTitle;

            //Act
            var result = _postService.UpdateAsync(expected).Result;

            //Arrange
            result.UpdatedAt.Should().NotBeNull();
        }

        [Test]
        public void Update_Should_Return_Updated_Results()
        {
            //Arrange
            var newTitle = "Modified Title";
            var expected = _mapper.Map<DM.Post>(_context.Posts.First());
            expected.Title = newTitle;

            //Act
            var result = _postService.UpdateAsync(expected).Result;
            expected.UpdatedAt = result.UpdatedAt;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }
    }
}