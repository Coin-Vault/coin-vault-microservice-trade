namespace TradingService.Dtos
{
    public class TradeReadDto
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }

        public string? Amount { get; set; }
        
        public string? Price { get; set; }
    }
}