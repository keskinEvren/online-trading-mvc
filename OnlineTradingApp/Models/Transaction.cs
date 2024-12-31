using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTradingApp.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public DateTime? TransactionDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        [Required]
        public int UserId { get; set; }

        [ForeignKey("ShareId")]
        public virtual Share? Share { get; set; }
        
        [Required]
        public int ShareId { get; set; }
    }
}
