using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TradingService.AsyncDataServices;
using TradingService.Controllers;
using TradingService.Data;
using TradingService.Dtos;
using TradingService.Models;
using TradingService.SyncDataServices.Http;
using Xunit;

namespace TradingService.Tests
{
    public class TradesControllerTests
    {
        private TradesController _tradesController;
        private Mock<ITradeRepo> _tradeRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IPortfolioDataClient> _portfolioDataClientMock;
        private Mock<IMessageBusClient> _messageBusClientMock;

        public TradesControllerTests()
        {
            _tradeRepoMock = new Mock<ITradeRepo>();
            _mapperMock = new Mock<IMapper>();
            _portfolioDataClientMock = new Mock<IPortfolioDataClient>();
            _messageBusClientMock = new Mock<IMessageBusClient>();

            _tradesController = new TradesController(
                _tradeRepoMock.Object,
                _mapperMock.Object,
                _portfolioDataClientMock.Object,
                _messageBusClientMock.Object
            );
        }

        [Fact]
        public void GetTradesByUserId_ValidUserId_ReturnsTradeReadDtos()
        {
            // Arrange
            string userId = "user1";
            var trades = new List<Trade>
            {
                new Trade { Id = 1, UserId = userId, Name = "Trade 1", Amount = 100 },
                new Trade { Id = 2, UserId = userId, Name = "Trade 2", Amount = 200 }
            };
            var tradeReadDtos = trades.Select(t => new TradeReadDto { Id = t.Id, UserId = t.UserId, Name = t.Name, Amount = t.Amount }).ToList();

            _tradeRepoMock.Setup(repo => repo.GetAllTradesByUserId(userId)).Returns(trades);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<TradeReadDto>>(trades)).Returns(tradeReadDtos);

            // Act
            var result = _tradesController.GetTradesByUserId(userId);

            // Assert
            Assert.IsAssignableFrom<ActionResult<IEnumerable<TradeReadDto>>>(result);

            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);

            var returnedTradeReadDtos = okResult.Value as IEnumerable<TradeReadDto>;
            Assert.Equal(tradeReadDtos.Count, returnedTradeReadDtos.Count());
            Assert.Equal(tradeReadDtos.First().Id, returnedTradeReadDtos.First().Id);
            Assert.Equal(tradeReadDtos.First().UserId, returnedTradeReadDtos.First().UserId);
            Assert.Equal(tradeReadDtos.First().Name, returnedTradeReadDtos.First().Name);
            Assert.Equal(tradeReadDtos.First().Amount, returnedTradeReadDtos.First().Amount);
        }


        [Fact]
        public async Task CreateTrade_ValidTradeCreateDto_ReturnsOkResult()
        {
            // Arrange
            var tradeCreateDto = new TradeCreateDto { UserId = "user1", Name = "Trade 1", Amount = 100 };
            var tradeModel = new Trade { UserId = tradeCreateDto.UserId, Name = tradeCreateDto.Name, Amount = (decimal?)tradeCreateDto.Amount };
            var tradeReadDto = new TradeReadDto { Id = tradeModel.Id, UserId = tradeModel.UserId, Name = tradeModel.Name, Amount = tradeModel.Amount };

            _mapperMock.Setup(mapper => mapper.Map<Trade>(tradeCreateDto)).Returns(tradeModel);
            _mapperMock.Setup(mapper => mapper.Map<TradeReadDto>(tradeModel)).Returns(tradeReadDto);

            // Act
            var result = await _tradesController.CreateTrade(tradeCreateDto);

            // Assert
            Assert.IsType<ActionResult<TradeReadDto>>(result);

            _tradeRepoMock.Verify(repo => repo.CreateTrade(tradeModel), Times.Once);
            _tradeRepoMock.Verify(repo => repo.SaveChanges(), Times.Once);

            _messageBusClientMock.Verify();
        }
    }
}
