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
using System.Threading.Tasks;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    internal class BoardServiceTests
    {
        private readonly List<Board> _mockData = new List<Board>()
        {
            new Board()
            {
                Id = 1,
                Title = "Test title",
                Description = "Test desc"
            }
        };

        private BoardService _boardService;

        private ReaktDbContext _context;

        private IMapper _mapper;

        [Test]
        public async Task Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<Domain.Models.Board>(_mockData.First(b => b.Id == 1));
            //Act
            var result = await _boardService.GetAsync(1);

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Domain.Models.Board>>(_mockData);
            //Act
            var result = await _boardService.GetAsync();

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
            _context.Boards.AddRange(_mockData);
            _context.SaveChanges();

            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new BoardProfile())));
            _boardService = new BoardService(_context, _mapper);
        }
    }
}