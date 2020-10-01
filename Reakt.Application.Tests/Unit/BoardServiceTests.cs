using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Reakt.Application.Persistence;
using Reakt.Application.Persistence.Models;
using Reakt.Application.Services;
using Reakt.Persistance.DataAccess;
using Reakt.Server.MapperConfig;

namespace Reakt.Application.Tests.Unit
{
    [TestFixture]
    class BoardServiceTests
    {
        private ReaktDbContext _context;
        private IMapper _mapper;
        private BoardService _boardService;

        private readonly List<Board> _mockData = new List<Board>()
        {
            new Board()
            {
                Id = 1,
                Title = "Test title",
                Description = "Test desc"
            }
        };
        
        [OneTimeSetUp]
        public void Setup()
        {
            //setup inmemorydb
            var x = new DbContextOptionsBuilder<ReaktDbContext>();
            x.UseInMemoryDatabase("UtDb");
            _context = new ReaktDbContext(x.Options);
            _context.Boards.AddRange(_mockData);
            _context.SaveChanges();

            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new BoardProfile())));
            _boardService = new BoardService(_context, _mapper);
        }

        [Test]
        public void Get_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<List<Domain.Models.Board>>(_mockData);
            //Act
            var result = _boardService.Get();

            //Arrange
            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void Get_by_Id_Should_Return_Results()
        {
            //Arrange
            var expected = _mapper.Map<Domain.Models.Board>(_mockData.First(b => b.Id == 1));
            //Act
            var result = _boardService.Get(1);

            //Arrange            
            result.Should().BeEquivalentTo(expected);
        }
    }
}

