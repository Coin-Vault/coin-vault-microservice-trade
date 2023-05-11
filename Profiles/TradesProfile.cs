using AutoMapper;
using TradingService.Dtos;
using TradingService.Models;

namespace TradingService.Profiles
{
    public class TradesProfile : Profile
    {
        public TradesProfile()
        {
            CreateMap<Trade, TradeReadDto>();
            CreateMap<TradeCreateDto, Trade>();
            CreateMap<TradeReadDto, TradePublishDto>();
        }
    }
}
