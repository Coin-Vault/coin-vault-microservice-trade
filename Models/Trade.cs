using System.ComponentModel.DataAnnotations;

namespace TradingService.Models
{
    public class Trade
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string? Name { get; set; }

        [Required]
        public double? Amount { get; set; }
    }
}
