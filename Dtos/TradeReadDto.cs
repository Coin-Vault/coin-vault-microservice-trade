namespace TradingService.Dtos
{
    public class TradeReadDto
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }

        public decimal? Amount { get; set; }
    }
}