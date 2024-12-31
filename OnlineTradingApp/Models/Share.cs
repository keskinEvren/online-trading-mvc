using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTradingApp.Models
{
    public class Share
    {
        [Key]
        public int ShareId { get; set; }

        [Required]
        [StringLength(3)]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Symbol must be exactly 3 uppercase characters.")]
        public string Symbol { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public virtual ICollection<PortfolioShare>? PortfolioShares { get; set; }

        public bool IsBuyable()
        {
            return Quantity > 0;
        }
    }
}
