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
        private EntityFactory<DM.Post> _entityFactory;
        private IMapper _mapper;
        private PostService _postService;

        [Test]
        public void AddAsync_Should_Add_Post_To_DbContext()
        {
            //Arrange
            var boardId = 1;
            var post = _entityFactory.BuildMock(5);
            post.BoardId = boardId;
          
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

            var post = _entityFactory.BuildMock(5);
            post.BoardId = boardId;

            //Act
            var result = _postService.AddAsync(boardId, post).Result;

            //Assert
            result.CreatedAt.Should().NotBeSameDateAs(new DateTime());
        }

        [Test]
        public void CreateAsync_Should_Throw_Exception()
        {
            //Arrange
            var post = _entityFactory.BuildMock(5);

            //Act-Assert
            _postService.Invoking(x => x.CreateAsync(post)).Should().Throw<NotImplementedException>();
        }

        [Test]
        public void Delete_Should_Mark_Items_Inactive()
        {
            //Arrange
            var id = 1;

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
            var id = 1;

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
            _entityFactory = new EntityFactory<DM.Post>();
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
            var expected = _mapper.Map<DM.Post>(_context.Posts.First(b => b.Id == 1));
            //Act
            var result = _postService.GetAsync(1).Result;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetForBoard_Should_Return_Results()
        {
            //Arrange
            var boardId = 1;
            var expected = _mapper.Map<List<DM.Post>>(_context.Posts.Where(x => x.BoardId == boardId));

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
            _context = MockDbContextFactory.BuildInMemory(_mapper.Map<List<PM.Post>>(_entityFactory.BuildMockList(1, 4)));
            _postService = new PostService(_context, _mapper);
        }

        [Test]
        public void Update_Should_Actualize_UpdatedAt_Property()
        {
            //Arrange
            var newTitle = "Modified Title";
            var expected = _mapper.Map<DM.Post>(_context.Posts.First(x => x.Id == 1));
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
            var expected = _mapper.Map<DM.Post>(_context.Posts.First(x => x.Id == 1));
            expected.Title = newTitle;

            //Act
            var result = _postService.UpdateAsync(expected).Result;
            expected.UpdatedAt = result.UpdatedAt;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }
    }
}