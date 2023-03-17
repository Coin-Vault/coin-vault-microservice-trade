using System.ComponentModel.DataAnnotations;

namespace TradingService.Dtos
{
    public class TradeCreateDto
    {   
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Amount { get; set; }
        
        [Required]
        public string? Price { get; set; }
    }
}