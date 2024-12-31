using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTradingApp.Models
{
    public class Portfolio
    {
        [Key]
        public int PortfolioId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Balance { get; set; }
        
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        

        public virtual ICollection<PortfolioShare>? PortfolioShares { get; set; }

        public bool IsBuyingOperationAffordable(int buyQuantity, decimal buyPrice)
        {
            return Balance >= buyQuantity * buyPrice;
        }
    }
}
