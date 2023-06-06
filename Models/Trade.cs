using System.ComponentModel.DataAnnotations;

namespace TradingService.Models
{
    public class Trade
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public decimal? Amount { get; set; }
    }
}
