using Microsoft.EntityFrameworkCore;
using TradingService.Models;

namespace TradingService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Trade>? Trades { get; set; }
    }
}