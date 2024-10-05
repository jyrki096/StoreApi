using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser User { get; set; }
        public DateTime OrderDateTime { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderDetails> OrderDetailsItems{ get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalCount { get; set; }
    }
}