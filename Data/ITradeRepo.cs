using TradingService.Models;

namespace TradingService.Data
{
    public interface ITradeRepo
    {
        bool SaveChanges();
        IEnumerable<Trade> GetAllTradesByUserId(string userId);
        Trade GetTradeById(int id);
        void CreateTrade(Trade trade);
    }
}