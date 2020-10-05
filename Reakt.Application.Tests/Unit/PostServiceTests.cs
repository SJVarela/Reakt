using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;
using Reakt.Application.Persistence.Models;
using Reakt.Application.Services;
using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    internal class PostServiceTests
    {
        private readonly List<Board> _boardData = new List<Board>()
        { new Board()
            {
                Id = 1,
                Title = "Only board",
                Description = "UT board",
                Posts = new List<Post>()
            }
        };

        private readonly List<Post> _postData = new List<Post>()
        {
            new Post()
            {
                Id = 1,
                Title = "Test title",
                Description = "Test desc",
                BoardId = 1,
                Active = true,
                UpdatedAt = new DateTime(2020,10,01,12,12,12),
                CreatedAt = new DateTime(2020,10,01,10,10,10),
                Comments = new List<Comment>()
            },
            new Post()
            {
                Id = 2,
                Title = "Test title 2",
                Description = "Test desc 2",
                BoardId = 1,
                Active = true,
                UpdatedAt = new DateTime(2020,10,01,12,12,12),
                CreatedAt = new DateTime(2020,10,01,10,10,10),
                Comments = new List<Comment>()
            },
            new Post()
            {
                Id = 3,
                Title = "Test 3",
                Description = "Test 3",
                BoardId = 2,
                Active = true,
                UpdatedAt = new DateTime(2020,10,01,12,12,12),
                CreatedAt = new DateTime(2020,10,01,10,10,10),
                Comments = new List<Comment>()
            },
        };

        private ReaktDbContext _context;
        private IMapper _mapper;
        private PostService _postService;

        [Test]
        public async Task AddAsync_Should_Add_Post_To_Board()
        {
            //Arrange
            var boardId = 1;
            var post = new Domain.Models.Post()
            {
                Title = "new created",
                Description = "new created",
                BoardId = boardId,
                Comments = new List<Domain.Models.Comment>()
            };

            //Act
            var result = await _postService.AddAsync(boardId, post);
            var board = _context.Boards.Include(b => b.Posts)
                                       .First(b => b.Id == result.BoardId);

            //Assert
            board.Posts.Should().Contain(p => p.Id == result.Id);
        }

        [Test]
        public async Task AddAsync_Should_Add_Post_To_DbContext()
        {
            //Arrange
            var boardId = 1;
            var post = new Domain.Models.Post()
            {
                Title = "new created",
                Description = "new created",
                BoardId = boardId,
                Comments = new List<Domain.Models.Comment>()
            };

            //Act
            var result = await _postService.AddAsync(boardId, post);

            //Assert
            result.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task AddAsync_Should_Set_CreatedAt_Property()
        {
            //Arrange
            var boardId = 1;
            var post = new Domain.Models.Post()
            {
                Title = "new created",
                Description = "new created",
                BoardId = boardId,
                Comments = new List<Domain.Models.Comment>()
            };

            //Act
            var result = await _postService.AddAsync(boardId, post);

            //Assert
            result.CreatedAt.Should().NotBeSameDateAs(new DateTime());
        }

        [Test]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task CreateAsync_Should_Throw_Exception()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Arrange
            var post = _postData.First();
            var domainPost = _mapper.Map<Domain.Models.Post>(post);

            //Act-Assert
            Assert.ThrowsAsync<NotImplementedException>(async () => await _postService.CreateAsync(domainPost));
        }

        [Test]
        public async Task Delete_Should_Mark_Items_Inactive()
        {
            //Arrange
            var id = 1;

            //Act
            await _postService.DeleteAsync(id);
            var isActive = _postData.Find(p => p.Id == id).Active;

            //Assert
            isActive.Should().BeFalse();
        }

        [Test]
        public async Task Delete_Should_Populate_DeletedAt_Property()
        {
            //Arrange
            var id = 1;

            //Act
            await _postService.DeleteAsync(id);
            var deletedAt = _postData.Find(p => p.Id == id).DeletedAt;

            //Assert
            deletedAt.Should().NotBeNull();
        }

        [Test]
        public async Task Get_by_Id_Should_Return_Null_When_Id_Not_Found()
        {
            //Arrange
            var invalidId = _postData.Max(p => p.Id) + 100;

            //Act
            var result = await _postService.GetAsync(invalidId);

            //Arrange
            result.Should().BeNull();
        }

        [Test]
        public async Task Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<Domain.Models.Post>(_postData.First(b => b.Id == 1));
            //Act
            var result = await _postService.GetAsync(1);

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Domain.Models.Post>>(_postData);
            //Act
            var result = await _postService.GetAsync();

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetForBoard_Should_Return_Results()
        {
            //Arrange
            var boardId = 1;
            var expected = _mapper.Map<List<Domain.Models.Post>>(_postData).FindAll(x => x.BoardId == boardId);

            //Act
            var result = await _postService.GetForBoardAsync(boardId);

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
            _context.Posts.AddRange(_postData);
            _context.Boards.AddRange(_boardData);
            _context.SaveChanges();

            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new PostProfile())));
            _postService = new PostService(_context, _mapper);
        }

        [Test]
        public async Task Update_Should_Actualize_UpdatedAt_Property()
        {
            //Arrange
            var newTitle = "Modified Title";
            var expected = _mapper.Map<List<Domain.Models.Post>>(_postData).First(x => x.Id == 1);
            expected.Title = newTitle;
            var oldDate = expected.UpdatedAt;

            //Act
            var result = await _postService.UpdateAsync(expected);

            //Arrange
            result.UpdatedAt.Should().NotBeNull();
            result.UpdatedAt.Should().BeAfter(oldDate.Value);
        }

        [Test]
        public async Task Update_Should_Return_Updated_Results()
        {
            //Arrange
            var newTitle = "Modified Title";
            var expected = _mapper.Map<List<Domain.Models.Post>>(_postData).First(x => x.Id == 1);
            expected.Title = newTitle;

            //Act
            var result = await _postService.UpdateAsync(expected);
            var wasUpdated = _context.Posts.Any(p => p.Title == newTitle);
            expected.UpdatedAt = result.UpdatedAt;

            //Arrange
            result.Should().BeEquivalentTo(expected);
            wasUpdated.Should().BeTrue();
        }
    }
}