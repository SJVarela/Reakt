using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Reakt.Application.Persistence.Models;
using Reakt.Application.Services;
using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void AddCommentAsync_Should_AddItem_Return_Results()
        {
            //Arrange
            var expected = new Domain.Models.Comment() { Message = "A new comment" };
            //Act
            var result = _commentService.AddCommentAsync(1, expected).Result;
            expected = _mapper.Map<Domain.Models.Comment>(_context.Comments.First(c => c.Id == result.Id));
            //Arrange
            result.Should().BeEquivalentTo(expected);
            _context.Comments.First(c => c.Id == result.Id);
        }

        [Test]
        public void DeleteAsync_Should_UpdateDate_Active_To_False()
        {
            //Arrange

            //Act
            _commentService.DeleteAsync(1).Wait();
            var expected = _context.Comments.FirstOrDefault(c => c.Id == 1);
            //Arrange
            expected.Should().BeNull();
        }

        [Test]
        public void Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<Domain.Models.Comment>(_mockData.First(b => b.Id == 1));
            //Act
            var result = _commentService.GetAsync(1).Result;

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Domain.Models.Comment>>(_mockData);
            //Act
            var result = _commentService.GetAsync().Result;

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
            _context.Comments.AddRange(_mockData);
            _context.SaveChanges();

            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new CommentProfile())));
            _commentService = new CommentService(_context, _mapper);
        }
    }
}