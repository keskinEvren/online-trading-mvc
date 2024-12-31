using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTradingApp.Models
{
    public class PortfolioShare
    {
        [Key]
        public int PortfolioShareId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("Portfolio")]
        public int PortfolioId { get; set; }

        [ForeignKey("Share")]
        public int ShareId { get; set; }

        public virtual Portfolio? Portfolio { get; set; }
        public virtual Share? Share { get; set; }

        public bool CanSellQuantity(int sellQuantity)
        {
            return Quantity >= sellQuantity;
        }
    }
}