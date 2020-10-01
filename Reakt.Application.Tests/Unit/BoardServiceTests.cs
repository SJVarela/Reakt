using System;
using System.Collections;
using System.Collections.Generic;
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
    //test change
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

        [SetUp]
        public void Setup()
        {
            //setup inmemorydb
            var x = new DbContextOptionsBuilder<ReaktDbContext>();
            x.UseInMemoryDatabase("UtDb");
            _context = new ReaktDbContext(x.Options);

            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new BoardProfile())));
            _boardService = new BoardService(_context, _mapper);
        }

        [Test]
        public void Get_Should_Return_Results()
        {
            //Arrange
            _context.Boards.AddRange(_mockData);
            _context.SaveChanges();

            //Act
            var result = _boardService.Get();

            //Arrange
            result.Should().BeEquivalentTo(_mapper.Map<List<Domain.Models.Board>>(_mockData));
            //CollectionAssert.AreEqual(_mapper.Map<List<Domain.Models.Board>>(_mockData), result, Comparer.Default);
        }
    }
}

