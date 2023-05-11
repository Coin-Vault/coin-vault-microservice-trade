using TradingService.Dtos;

namespace TradingService.SyncDataServices.Http
{
    public interface IPortfolioDataClient
    {
        Task SendTradeToPortfolio(TradeReadDto trade);
    }

}