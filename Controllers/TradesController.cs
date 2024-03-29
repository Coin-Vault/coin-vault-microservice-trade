using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingService.AsyncDataServices;
using TradingService.Data;
using TradingService.Dtos;
using TradingService.Models;
using TradingService.SyncDataServices.Http;

namespace TradingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly ITradeRepo _repository;
        private readonly IMapper _mapper;
        private readonly IPortfolioDataClient _portfolioDataClient;
        private readonly IMessageBusClient _IMessageBusClient;

        public TradesController(ITradeRepo repository, IMapper mapper, IPortfolioDataClient portfolioDataClient, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _portfolioDataClient = portfolioDataClient;
            _IMessageBusClient = messageBusClient;
        }

        [Authorize]
        [HttpGet("{userId}", Name = "GetTradesByUserId")]
        public ActionResult<IEnumerable<TradeReadDto>> GetTradesByUserId(string userId)
        {
            Console.WriteLine("Getting Trades");

            var trades = _repository.GetAllTradesByUserId(userId);

            return Ok(_mapper.Map<IEnumerable<TradeReadDto>>(trades));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TradeReadDto>> CreateTrade(TradeCreateDto tradeCreateDto)
        {
            var tradeModel = _mapper.Map<Trade>(tradeCreateDto);
            _repository.CreateTrade(tradeModel);
            _repository.SaveChanges();

            var tradeReadDto = _mapper.Map<TradeReadDto>(tradeModel);

            try
            {
                await _portfolioDataClient.SendTradeToPortfolio(tradeReadDto);
            }
            catch (Exception exeption)
            {
                Console.WriteLine($"Could not send POST data: {exeption.Message}");
            }

            try
            {
                var tradePublishDto = _mapper.Map<TradePublishDto>(tradeReadDto);
                tradePublishDto.Price = 25000;
                tradePublishDto.Event = "Trade_Publish";

                _IMessageBusClient.PublishNewTrade(tradePublishDto);
            }
            catch (Exception exeption)
            {
                Console.WriteLine($"Could not send RabbitMQ data: {exeption.Message}");
            }

            return Ok();
        }
    }
}