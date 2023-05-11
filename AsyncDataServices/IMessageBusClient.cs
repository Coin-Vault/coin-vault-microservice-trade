using TradingService.Dtos;

namespace TradingService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewTrade(TradePublishDto tradePublishDto);
    }
}