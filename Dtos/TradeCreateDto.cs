using System.ComponentModel.DataAnnotations;

namespace TradingService.Dtos
{
    public class TradeCreateDto
    {   
        [Required]
        public string? UserId { get; set; }
        
        [Required]
        public string? Name { get; set; }

        [Required]
        public double? Amount { get; set; }
    }
}