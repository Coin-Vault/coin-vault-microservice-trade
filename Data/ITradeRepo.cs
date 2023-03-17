using TradingService.Models;

namespace TradingService.Data
{
    public interface ITradeRepo
    {
        bool SaveChanges();
        IEnumerable<Trade> GetAllTrades();
        Trade GetTradeById(int id);
        void CreateTrade(Trade trade);
    }
}