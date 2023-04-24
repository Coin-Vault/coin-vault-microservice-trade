using System.ComponentModel.DataAnnotations;

namespace TradingService.Dtos
{
    public class TradeCreateDto
    {   
        [Required]
        public string? Name { get; set; }

        [Required]
        public double? Amount { get; set; }
    }
}