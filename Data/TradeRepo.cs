using TradingService.Models;

namespace TradingService.Data
{
    public class TradeRepo : ITradeRepo
    {
        private readonly AppDbContext _context;

        public TradeRepo(AppDbContext context)
        {
            _context = context;       
        }

        public void CreateTrade(Trade trade)
        {
            if(trade == null)
            {
                throw new ArgumentNullException(nameof(trade));
            }

            _context.Trades.Add(trade);
        }

        public IEnumerable<Trade> GetAllTradesByUserId(string userId)
        {
            return _context.Trades.Where(t => t.UserId == userId);
        }

        public Trade GetTradeById(int id)
        {
            return _context.Trades.FirstOrDefault(t => t.Id == id);
        }

        public bool SaveChanges()
        {
            return(_context.SaveChanges() >= 0);
        }
    }
}