using System.ComponentModel.DataAnnotations;

namespace OnlineTradingApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserFirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserLastName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        
        
        public virtual Portfolio? Portfolio { get; set; }
    }
}
