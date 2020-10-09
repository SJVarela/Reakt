using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Reakt.Application.Persistence.Models;
using Reakt.Application.Services;
using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    public class PostServiceTests
    {
        private readonly ReadOnlyCollection<Board> _boardData = new ReadOnlyCollection<Board>(new List<Board>()
        { new Board()
            {
                Id = 1,
                Title = "Only board",
                Description = "UT board",
                Posts = new List<Post>()
            }
        });

        private readonly ReadOnlyCollection<Post> _postData = new ReadOnlyCollection<Post>(new List<Post>()
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
        });

        private ReaktDbContext _context;
        private IMapper _mapper;
        private PostService _postService;

        private ReaktDbContext InitializeContext(string storeName)
        {
            //setup inmemorydb
            var context = new ReaktDbContext(
                new DbContextOptionsBuilder<ReaktDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            context.Posts.AddRange(_postData);
            context.Boards.AddRange(_boardData);
            context.SaveChanges();

            return context;
        }

        //[Test]
        //public void AddAsync_Should_Add_Post_To_Board()
        //{
        //    //Arrange
        //    var boardId = 1;
        //    var post = new Domain.Models.Post()
        //    {
        //        Title = "new created",
        //        Description = "new created",
        //        BoardId = boardId,
        //        Comments = new List<Domain.Models.Comment>()
        //    };

        //    //Act
        //    var result = _postService.AddAsync(boardId, post).Result;
        //    var board = _context.Boards.Include(b => b.Posts)
        //                               .First(b => b.Id == result.BoardId);

        //    //Assert
        //    board.Posts.Should().Contain(p => p.Id == result.Id);
        //}

        [Test]
        public void AddAsync_Should_Add_Post_To_DbContext()
        {
            //Arrange
            var boardId = 1;
            var post = new Domain.Models.Post()
            {
                Title = "new created",
                Description = "new created",
                Comments = new List<Domain.Models.Comment>()
            };

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
            var post = new Domain.Models.Post()
            {
                Title = "new created",
                Description = "new created",
                Comments = new List<Domain.Models.Comment>()
            };

            //Act
            var result = _postService.AddAsync(boardId, post).Result;

            //Assert
            result.CreatedAt.Should().NotBeSameDateAs(new DateTime());
        }

        [Test]
        [Ignore("Problemas con async")]
        public void CreateAsync_Should_Throw_Exception()
        {
            //Arrange
            var post = _postData.First();
            var domainPost = _mapper.Map<Domain.Models.Post>(post);

            //Act-Assert
            Assert.Throws<NotImplementedException>(async () => await _postService.CreateAsync(domainPost));
        }

        [Test]
        public void Delete_Should_Mark_Items_Inactive()
        {
            //Arrange
            var id = 1;

            //Act
            _postService.DeleteAsync(id).Wait();
            var isActive = _postData.First(p => p.Id == id).Active;

            //Assert
            isActive.Should().BeFalse();
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

        [Test]
        public void Get_by_Id_Should_Return_Null_When_Id_Not_Found()
        {
            //Arrange
            var invalidId = _postData.Max(p => p.Id) + 100;

            //Act
            var result = _postService.GetAsync(invalidId).Result;

            //Arrange
            result.Should().BeNull();
        }

        [Test]
        public void Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<Domain.Models.Post>(_postData.First(b => b.Id == 1));
            //Act
            var result = _postService.GetAsync(1).Result;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        [Ignore("TODO: Affected by AddAsync UTs... WHY ?")]
        public void Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Domain.Models.Post>>(_postData);
            //Act
            var result = _postService.GetAsync().Result;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        [Ignore("TODO: Affected by AddAsync UTs... WHY ?")]
        public void GetForBoard_Should_Return_Results()
        {
            //Arrange
            var boardId = 1;
            var expected = _mapper.Map<List<Domain.Models.Post>>(_postData.Where(x => x.BoardId == boardId));

            //Act
            var result = _postService.GetForBoardAsync(boardId, 0, 50).Result;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [SetUp]
        public void Setup()
        {
            //setup inmemorydb
            _context = new ReaktDbContext(
                new DbContextOptionsBuilder<ReaktDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            _context.Posts.AddRange(_postData);
            _context.SaveChanges();

            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new PostProfile())));
            _postService = new PostService(_context, _mapper);
        }

        [Test]
        public void Update_Should_Actualize_UpdatedAt_Property()
        {
            //Arrange
            var newTitle = "Modified Title";
            var expected = _mapper.Map<Domain.Models.Post>(_postData.First(x => x.Id == 1));
            expected.Title = newTitle;
            var oldDate = expected.UpdatedAt;

            //Act
            var result = _postService.UpdateAsync(expected).Result;

            //Arrange
            result.UpdatedAt.Should().NotBeNull();
            result.UpdatedAt.Should().BeAfter(oldDate.Value);
        }

        [Test]
        public void Update_Should_Return_Updated_Results()
        {
            //Arrange
            var newTitle = "Modified Title";
            var expected = _mapper.Map<Domain.Models.Post>(_postData.First(x => x.Id == 1));
            expected.Title = newTitle;

            //Act
            var result = _postService.UpdateAsync(expected).Result;
            expected.UpdatedAt = result.UpdatedAt;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }
    }
}