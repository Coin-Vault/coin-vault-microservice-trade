using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TradingService.Data;
using TradingService.Dtos;
using TradingService.Models;

namespace TradingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly ITradeRepo _repository;
        private readonly IMapper _mapper;

        public TradesController(ITradeRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TradeReadDto>> GetTrades()
        {
            Console.WriteLine("Getting Trades");

            var trades = _repository.GetAllTrades();

            return Ok(_mapper.Map<IEnumerable<TradeReadDto>>(trades));
        }

        [HttpGet("{id}", Name = "GetTradeById")]
        public ActionResult<TradeReadDto> GetTradeById(int id)
        {
            var trade = _repository.GetTradeById(id);

            if (trade != null) {
                return Ok(_mapper.Map<TradeReadDto>(trade));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<TradeReadDto> CreateTrade(TradeCreateDto tradeCreateDto)
        {
            var tradeModel = _mapper.Map<Trade>(tradeCreateDto);
            _repository.CreateTrade(tradeModel);
            _repository.SaveChanges();

            var tradeReadDto = _mapper.Map<TradeReadDto>(tradeModel);

            return CreatedAtRoute(nameof(GetTradeById), new { Id = tradeReadDto.Id}, tradeReadDto);
        }
    }
}